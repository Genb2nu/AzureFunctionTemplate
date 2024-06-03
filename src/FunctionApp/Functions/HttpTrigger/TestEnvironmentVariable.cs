using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HttpTrigger.Functions.HttpTrigger
{
    public class TestEnvironmentVariable
    {
        private readonly ILogger<TestEnvironmentVariable> _logger;
        private readonly IConfiguration _configuration;

        public TestEnvironmentVariable(ILogger<TestEnvironmentVariable> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [Function("TestEnvironmentVariable")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            // this is to test environment variables:
            string? environmentVariable = _configuration.GetValue<string?>("test");

            // expected: value from environment variable (locally: local.seeting.json),
            // override value from appsettings.Developemnt.json and appsettings.json
            return new OkObjectResult(environmentVariable);
        }
    }
}
