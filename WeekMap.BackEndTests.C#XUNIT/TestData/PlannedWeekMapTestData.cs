using WeekMap.DTOs;
using XUnitTests.TestData;

public class PlannedWeekMapTestData : ITestData<PlannedWeekMapDTO>
{
    public IEnumerable<PlannedWeekMapDTO> Valid =>
    new List<PlannedWeekMapDTO>()
    {
        new PlannedWeekMapDTO
        {
            Name = "name1",
            DayStartTime = new TimeSpan(8, 0, 0),
            DayEndTime = new TimeSpan(16, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            Name = "name2",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(23, 59, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = false,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            Name = "name2",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(1, 0, 0),
            ShowPlaceInPreview = false,
            ShowDescriptionInPreview = false,
            UserID = 1
        }
    };

    public IEnumerable<PlannedWeekMapDTO> Invalid =>
    new List<PlannedWeekMapDTO>()
    {
        new PlannedWeekMapDTO
        {
            Name = "name3",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(0, 0, 0),
            ShowPlaceInPreview = false,
            ShowDescriptionInPreview = false,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            Name = "name4",
            DayStartTime = new TimeSpan(-8, 0, 0),
            DayEndTime = new TimeSpan(16, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            Name = "name6",
            DayStartTime = new TimeSpan(16, 0, 0),
            DayEndTime = new TimeSpan(8, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            Name = "name7",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(23, 58, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        },
        new PlannedWeekMapDTO
        {
            Name = "name8",
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(24, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true,
            UserID = 1
        }
    };
}