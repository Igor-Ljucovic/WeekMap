
using WeekMap.DTOs;
using XUnitTests.TestData;

public class UserDefaultWeekMapSettingsTestData : ITestData<UserDefaultWeekMapSettingsDTO>
{
    public IEnumerable<UserDefaultWeekMapSettingsDTO> Valid => new List<UserDefaultWeekMapSettingsDTO>
    {
        new UserDefaultWeekMapSettingsDTO
        {   
            UserID = 1,
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(23, 59, 59),
            ShowPlaceInPreview = false,
            ShowDescriptionInPreview = false
        },
        new UserDefaultWeekMapSettingsDTO
        {   
            UserID = 1,
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(23, 59, 0),
            ShowPlaceInPreview = false,
            ShowDescriptionInPreview = true
        },
        new UserDefaultWeekMapSettingsDTO
        {   
            UserID = 1,
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(6, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true
        },
    };

    public IEnumerable<UserDefaultWeekMapSettingsDTO> Invalid => new List<UserDefaultWeekMapSettingsDTO>
    {
        new UserDefaultWeekMapSettingsDTO
        {
            UserID = 1,
            DayStartTime = new TimeSpan(14, 0, 0),
            DayEndTime = new TimeSpan(3, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true
        },
        new UserDefaultWeekMapSettingsDTO
        {
            UserID = 1,
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(24, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true
        },
        new UserDefaultWeekMapSettingsDTO
        {
            UserID = 1,
            DayStartTime = new TimeSpan(23, 0, 0),
            DayEndTime = new TimeSpan(0, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true
        },
        new UserDefaultWeekMapSettingsDTO
        {
            UserID = 1,
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(-1, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true
        },
        new UserDefaultWeekMapSettingsDTO
        {
            UserID = 1,
            DayStartTime = new TimeSpan(0, 0, 0),
            DayEndTime = new TimeSpan(12, 30, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true
        },
        new UserDefaultWeekMapSettingsDTO
        {
            UserID = 1,
            DayStartTime = new TimeSpan(-1, 0, 0),
            DayEndTime = new TimeSpan(8, 0, 0),
            ShowPlaceInPreview = true,
            ShowDescriptionInPreview = true
        },
    };
}
