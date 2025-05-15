using WeekMap.DTOs;
using XUnitTests.TestData;

public class PlannedWeekMapActivityTestData : ITestData<PlannedWeekMapActivityDTO>
{
    public IEnumerable<PlannedWeekMapActivityDTO> Valid =>
    new List<PlannedWeekMapActivityDTO>
    {
        new PlannedWeekMapActivityDTO {
            ActivityID = 1,
            PlannedWeekMapID = 1,
            StartTime = new TimeSpan(0, 0, 0),
            EndTime = new TimeSpan(1, 30, 0),
            OnMonday = true,
            OnTuesday = true,
            OnWednesday = true,
            OnThursday = true,
            OnFriday = true,
            OnSaturday = true,
            OnSunday = true
        },
        new PlannedWeekMapActivityDTO {
            ActivityID = 1,
            PlannedWeekMapID = 1,
            StartTime = new TimeSpan(0, 0, 0),
            EndTime = new TimeSpan(23, 0, 0),
            OnMonday = false,
            OnTuesday = false,
            OnWednesday = false,
            OnThursday = false,
            OnFriday = false,
            OnSaturday = false,
            OnSunday = false
        },
        new PlannedWeekMapActivityDTO {
            ActivityID = 1,
            PlannedWeekMapID = 1,
            StartTime = new TimeSpan(3, 30, 0),
            EndTime = new TimeSpan(4, 0, 0),
            OnMonday = false,
            OnTuesday = false,
            OnWednesday = false,
            OnThursday = true,
            OnFriday = true,
            OnSaturday = true,
            OnSunday = true
        },
        new PlannedWeekMapActivityDTO {
            ActivityID = 1,
            PlannedWeekMapID = 1,
            StartTime = new TimeSpan(0, 0, 0),
            EndTime = new TimeSpan(1, 0, 0),
            OnMonday = false,
            OnTuesday = true,
            OnWednesday = true,
            OnThursday = true,
            OnFriday = true,
            OnSaturday = true,
            OnSunday = true
        },
    };

    public IEnumerable<PlannedWeekMapActivityDTO> Invalid =>
    new List<PlannedWeekMapActivityDTO>
    {
        new PlannedWeekMapActivityDTO {
            ActivityID = 1,
            PlannedWeekMapID = 1,
            StartTime = new TimeSpan(0, 0, 0),
            EndTime = new TimeSpan(-1, 0, 0),
            OnMonday = true,
            OnTuesday = true,
            OnWednesday = true,
            OnThursday = true,
            OnFriday = true,
            OnSaturday = true,
            OnSunday = true
        },
        new PlannedWeekMapActivityDTO {
            ActivityID = 1,
            PlannedWeekMapID = 1,
            StartTime = new TimeSpan(0, 0, 0),
            EndTime = new TimeSpan(24, 0, 0),
            OnMonday = true,
            OnTuesday = true,
            OnWednesday = true,
            OnThursday = true,
            OnFriday = true,
            OnSaturday = true,
            OnSunday = true
        },
        new PlannedWeekMapActivityDTO {
            ActivityID = 1,
            PlannedWeekMapID = 1,
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(5, 0, 0),
            OnMonday = true,
            OnTuesday = true,
            OnWednesday = true,
            OnThursday = true,
            OnFriday = true,
            OnSaturday = true,
            OnSunday = true
        }
    };
}
