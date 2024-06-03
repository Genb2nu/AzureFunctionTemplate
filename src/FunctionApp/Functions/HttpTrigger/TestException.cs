using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HttpTrigger.Functions.HttpTrigger
{
    public class TestException
    {
        private readonly ILogger<TestException> _logger;
        private readonly IConfiguration _configuration;

        public TestException(ILogger<TestException> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [Function("TestException")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            throw new System.Exception("This is a test exception");
        }
    }
}
