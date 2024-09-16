using System;
using BpClockerAzureFunction.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BpClockerAzureFunction
{
    public class ClockerCron
    {
        private readonly ILogger _logger;
        private readonly IClockService _clockService;

        public ClockerCron(ILoggerFactory loggerFactory, IClockService clockService)
        {
            _logger = loggerFactory.CreateLogger<ClockerCron>();
            _clockService = clockService;
        }

        [Function("ClockIn")]
        public void Run([TimerTrigger("0 0 9 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# ClockIn trigger, Start at: {DateTime.Now}");
            _clockService.ClockIn();
            _logger.LogInformation($"C# ClockIn trigger, End at: {DateTime.Now}");
        }
    }
}
