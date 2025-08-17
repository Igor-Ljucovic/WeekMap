using WeekMap.DTOs;
using XUnitTests.TestData;

public class WeekMapActivityTestData : ITestData<WeekMapActivityDTO>
{
    public IEnumerable<WeekMapActivityDTO> Valid =>
    new List<WeekMapActivityDTO>
    {
        new WeekMapActivityDTO {
            ActivityTemplateID = 1,
            WeekMapID = 1,
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
        new WeekMapActivityDTO {
            ActivityTemplateID = 1,
            WeekMapID = 1,
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
        new WeekMapActivityDTO {
            ActivityTemplateID = 1,
            WeekMapID = 1,
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
        new WeekMapActivityDTO {
            ActivityTemplateID = 1,
            WeekMapID = 1,
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

    public IEnumerable<WeekMapActivityDTO> Invalid =>
    new List<WeekMapActivityDTO>
    {
        new WeekMapActivityDTO {
            ActivityTemplateID = 1,
            WeekMapID = 1,
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
        new WeekMapActivityDTO {
            ActivityTemplateID = 1,
            WeekMapID = 1,
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
        new WeekMapActivityDTO {
            ActivityTemplateID = 1,
            WeekMapID = 1,
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
