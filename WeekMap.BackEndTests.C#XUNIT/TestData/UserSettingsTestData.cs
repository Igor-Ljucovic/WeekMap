using WeekMap.DTOs;
using XUnitTests.TestData;

public class UserSettingsTestData : ITestData<UserSettingsDTO>
{
    public IEnumerable<UserSettingsDTO> Valid => new List<UserSettingsDTO>
    {
        new UserSettingsDTO { UserID = 1, Theme = "dark" },
        new UserSettingsDTO { UserID = 1, Theme = "light"}
    };

    public IEnumerable<UserSettingsDTO> Invalid => new List<UserSettingsDTO>
    {
        new UserSettingsDTO { UserID = 1, Theme = "", },
        new UserSettingsDTO { UserID = 1, Theme = "ligh", },
        new UserSettingsDTO { UserID = 1, Theme = "ight", },
        new UserSettingsDTO { UserID = 1, Theme = "dark ", },
        new UserSettingsDTO { UserID = 1, Theme = " dark", },
        new UserSettingsDTO { UserID = 1, Theme = "Light" },
        new UserSettingsDTO { UserID = 1, Theme = "asdf", },
        new UserSettingsDTO { UserID = 1, Theme = "Dark", }
    };
}
