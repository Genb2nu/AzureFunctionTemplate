using Core.Business.Entities;
using Core.Business.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Formats.Asn1;
using System.Globalization;
using System.Data.SqlClient;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using Renci.SshNet;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace HttpTrigger.Functions.TimeTrigger
{
    public class DataTransferJob
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public DataTransferJob(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<DataTransferJob>();
            _configuration = configuration;
        }

        /// <summary>
        /// Triggers Data Transfer Job 1st of every month
        /// </summary>
        /// <param name="myTimer"></param>
        [Function("DataTransferJob")]
        public void Run([TimerTrigger("0 0 0 1 * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Timer trigger function executed at: {DateTime.UtcNow}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
            try
            {
                string synapseConnectionString = _configuration.GetConnectionString("AppDbContext");
                string query = _configuration.GetValue<string>("SynapseQuery");
                string sftpHost = _configuration.GetValue<string>("SFTPHost");
                string sftpUsername = _configuration.GetValue<string>("SFTPUsername");
                string sftpPassword = _configuration.GetValue<string>("SFTPPassword");
                string remoteCsvFilePath = _configuration.GetValue<string>("RemoteCsvFilePath");

                // Pull data from Azure Synapse Gold Tier and create CSV in memory
                using (SqlConnection conn = new SqlConnection(synapseConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    using (var memoryStream = new MemoryStream())
                    using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csvWriter.WriteRecords(reader);
                        streamWriter.Flush();
                        memoryStream.Position = 0;

                        // Upload the CSV file from memory to Azure SFTP VM
                        using (var client = new SftpClient(sftpHost, sftpUsername, sftpPassword))
                        {
                            client.Connect();
                            client.UploadFile(memoryStream, remoteCsvFilePath);
                            client.Disconnect();
                        }
                    }
                }
                _logger.LogInformation("Data successfully pulled from Synapse, saved to CSV, and uploaded to SFTP.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}
