namespace BpClockerAzureFunction.Dtos;

public class BpConfigs
{
    public required string ApiBase { get; set; }
    public string ClockApiPath { get; set; } = "";
    public List<string> CardIds { get; set; } = [];
}
