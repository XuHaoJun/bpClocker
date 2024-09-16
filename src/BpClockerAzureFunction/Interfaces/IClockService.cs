namespace BpClockerAzureFunction.Interfaces;

public interface IClockService
{
    Task<bool> ClockIn();

    Task<bool> IsHoliday(DateTime date);
}
