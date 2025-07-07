using WeekMap.DTOs;
using XUnitTests.TestData;

public class PlannedWeekMapTestData : ITestData<PlannedWeekMapDTO>
{
    public IEnumerable<PlannedWeekMapDTO> Valid =>
    new List<PlannedWeekMapDTO>()
    {
        new PlannedWeekMapDTO
        {
            WeekStartDay = "Monday",
            DayStartTime = new TimeSpan(8, 0, 0),
            DayEndTime = new TimeSpan(16, 0, 0),
            ShowSaturday = false,
            ShowSunday = false,
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            WeekStartDay = "Tuesday",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(24, 0, 0),
            ShowSaturday = true,
            ShowSunday = false,
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
    };

    public IEnumerable<PlannedWeekMapDTO> Invalid =>
    new List<PlannedWeekMapDTO>()
    {
        new PlannedWeekMapDTO
        {
            WeekStartDay = "saturday",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(0, 0, 0),
            ShowSaturday = false,
            ShowSunday = false,
            ShowPlaceInPreview = false,
            ShowDescriptionInPreview = false,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            WeekStartDay = "ThuRSdaY",
            DayStartTime = new TimeSpan(8, 0, 0),
            DayEndTime = new TimeSpan(16, 0, 0),
            ShowSaturday = true,
            ShowSunday = true,
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            WeekStartDay = "FRIDAY",
            DayStartTime = new TimeSpan(8, 0, 0),
            DayEndTime = new TimeSpan(16, 0, 0),
            ShowSaturday = true,
            ShowSunday = true,
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            WeekStartDay = "Saturday",
            DayStartTime = new TimeSpan(16, 0, 0),
            DayEndTime = new TimeSpan(8, 0, 0),
            ShowSaturday = true,
            ShowSunday = true,
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            WeekStartDay = "Saturday",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(0, 0, 0),
            ShowSaturday = true,
            ShowSunday = true,
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            WeekStartDay = "Saturday",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(25, 0, 0),
            ShowSaturday = true,
            ShowSunday = true,
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        }
    };
}