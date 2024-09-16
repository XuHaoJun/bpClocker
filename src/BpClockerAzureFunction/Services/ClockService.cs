using BpClockerAzureFunction.Dtos;
using BpClockerAzureFunction.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace BpClockerAzureFunction.Services;

public class ClockService : IClockService
{
    private readonly ILogger<ClockService> _logger;
    private readonly BpConfigs _bpConfigs;

    public ClockService(ILogger<ClockService> logger, IOptionsSnapshot<BpConfigs> bpConfigs)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(bpConfigs.Value);
        _logger = logger;
        _bpConfigs = bpConfigs.Value;
    }

    public void ClockIn()
    {
        var client = new RestClient(new RestClientOptions(_bpConfigs.ApiBase));

        foreach (var cardId in _bpConfigs.CardIds)
        {
            var request = new RestRequest(_bpConfigs.ClockApiPath, Method.Post);
            request.AddParameter("DeptNo", "");
            request.AddParameter("ClockInResult", "");
            // 0 上班, 1 下班
            request.AddParameter("ClockType", "0");
            request.AddParameter("OverHour", "0");
            request.AddParameter("EmpNo", "");
            request.AddParameter("isMeal", "0");
            request.AddParameter("ShiftID", "");
            request.AddParameter("ClockInMsg", "");
            request.AddParameter("IsOfficialBusiness", "False");
            request.AddParameter("IsShowOfficialBusiness", "False");
            request.AddParameter("CarID2", "");
            request.AddParameter("TempClockType", "");
            request.AddParameter("IsDisableClockInAndOut", "False");
            request.AddParameter("IsPreOvClockInAndOut", "False");
            request.AddParameter("CardId", cardId);
            try
            {
                var resp = client.Execute(request);
                if (resp.ErrorException != null)
                {
                    throw resp.ErrorException;
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning(e.ToString());
                throw;
            }
        }
    }
}
