using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BpClockerAzureFunction
{
    public class ClockerCron
    {
        private readonly ILogger _logger;

        public ClockerCron(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ClockerCron>();
        }

        [Function("ClockIn")]
        public void Run([TimerTrigger("0 0 9 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
