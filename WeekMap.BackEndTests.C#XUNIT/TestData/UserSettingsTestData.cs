using WeekMap.DTOs;
using XUnitTests.TestData;

public class UserSettingsTestData : ITestData<UserSettingsDTO>
{
    public IEnumerable<UserSettingsDTO> Valid => new List<UserSettingsDTO>
    {
        new UserSettingsDTO { UserID = 1, Theme = "dark", Notification = true, NotificationTime = new TimeSpan(0, 0, 0), EmailUpdates = true },
        new UserSettingsDTO { UserID = 1, Theme = "light", Notification = false, NotificationTime = new TimeSpan(21, 30, 0), EmailUpdates = false }
    };

    public IEnumerable<UserSettingsDTO> Invalid => new List<UserSettingsDTO>
    {
        new UserSettingsDTO { UserID = 1, Theme = "light", Notification = false, NotificationTime = new TimeSpan(25, 0, 0), EmailUpdates = true },
        new UserSettingsDTO { UserID = 1, Theme = "light", Notification = false, NotificationTime = new TimeSpan(-2, 0, 0), EmailUpdates = true },
        new UserSettingsDTO { UserID = 1, Theme = "asdf", Notification = true, NotificationTime = new TimeSpan(9, 0, 0), EmailUpdates = false },
        new UserSettingsDTO { UserID = 1, Theme = "Dark", Notification = true, NotificationTime = new TimeSpan(9, 0, 0), EmailUpdates = false }
    };
}
