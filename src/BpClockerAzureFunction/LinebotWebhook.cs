using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BpClockerAzureFunction
{
    public class LinebotWebhook
    {
        private readonly ILogger<LinebotWebhook> _logger;

        public LinebotWebhook(ILogger<LinebotWebhook> logger)
        {
            _logger = logger;
        }

        [Function("LinebotWebhook")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
