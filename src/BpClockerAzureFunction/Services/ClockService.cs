using System.Globalization;
using BpClockerAzureFunction.Dtos;
using BpClockerAzureFunction.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using HtmlAgilityPack;
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

    public async Task<bool> ClockIn()
    {
        var now = DateTime.Now;
        if (await IsHoliday(now))
        {
            // TODO
            // line or email it
            return false;
        }
        else
        {
            DoClock(new DoClockConfigs() { ClockType = "0" });
            // TODO
            // line or email it
            return true;
        }
    }

    public void DoClock(DoClockConfigs config)
    {
        var client = new RestClient(new RestClientOptions(_bpConfigs.ApiBase));

        foreach (var cardId in _bpConfigs.CardIds)
        {
            var request = new RestRequest(_bpConfigs.ClockApiPath, Method.Post);
            request.AddParameter("DeptNo", "");
            request.AddParameter("ClockInResult", "");
            // 0 上班, 1 下班
            request.AddParameter("ClockType", config.ClockType);
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

    public async Task<bool> IsHoliday(DateTime date)
    {
        int rocYear = WesternToRocYear(date.Year);

        // 中華民國政府行政機關辦公日曆表
        string url = "https://data.gov.tw/dataset/14718"; // Replace with the URL of the page you want to parse
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load(url);

        // Extract URLs with .csv suffix and title="CSV下載檔案"
        List<string> csvUrls = doc.DocumentNode
            .SelectNodes("//a[@href and @title='CSV下載檔案']")
            .Select(node => node.GetAttributeValue("href", string.Empty))
            .Where(href => href.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var noGoogleCsvUrls = csvUrls.Where(href => !href.Contains("Google"));
        var csvFileUrl = noGoogleCsvUrls.FirstOrDefault(href => href.Contains($"name={rocYear}"));
        if (csvFileUrl == null)
        {
            throw new Exception("csvFileUrl is null");
        }
        var handler = new HttpClientHandler() { AllowAutoRedirect = false };
        using HttpClient client = new(handler);
        client.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/csv")
        );
        var csvResp = await client.GetStringAsync(csvFileUrl);
        var holidayData = ParseCsv(csvResp);
        var record = holidayData.FirstOrDefault(x => x.西元日期 == date.ToString("yyyyMMdd"));
        if (record == null)
        {
            return false;
        }
        else
        {
            return record.是否放假 == "2";
        }
    }

    private static List<HolidayRecord> ParseCsv(string csvData)
    {
        using var reader = new StringReader(csvData);
        var readConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
        };
        using var csv = new CsvReader(reader, readConfiguration);
        var records = new List<HolidayRecord>();
        records = csv.GetRecords<HolidayRecord>().ToList();
        return records;
    }

    private static int WesternToRocYear(int year)
    {
        if (year <= 1911)
        {
            return 0;
        }
        else
        {
            return year - 1911;
        }
    }
}

public class DoClockConfigs
{
    public string ClockType { get; set; } = "0";
}
