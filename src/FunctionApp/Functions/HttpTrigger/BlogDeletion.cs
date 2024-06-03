using Core.Business.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpTrigger.Functions.HttpTrigger
{
    public class BlogDeletion
    {
        private readonly ILogger<BlogDeletion> _logger;
        private readonly IBlogService _blogService;

        public BlogDeletion(ILogger<BlogDeletion> logger, IBlogService blogService)
        {
            _logger = logger;
            _blogService = blogService;
        }

        [Function("DeleteBlog")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            long blogId = req.Query.ContainsKey("blogId") ? Convert.ToInt64(req.Query["blogId"]) : 0;
            var deletionRes = await _blogService.DeleteBlog(blogId);
            return new OkObjectResult(deletionRes);
        }
    }
}