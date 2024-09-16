// Class to represent the data structure for the CSV
public class HolidayRecord
{
    public required string 西元日期 { get; set; } // Date in the CSV
    public required string 星期 { get; set; } // Day of the week
    public required string 是否放假 { get; set; } // Holiday indicator (0 or 2)
    public string 備註 { get; set; } = ""; // Notes (e.g., holiday name)
}
