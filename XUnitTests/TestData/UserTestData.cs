using WeekMap.DTOs;
using XUnitTests.TestData;

public class UserTestData : ITestData<RegisterDTO>
{
    public IEnumerable<RegisterDTO> Valid
    {
        get => new List<RegisterDTO>
        {
            new RegisterDTO { Username = "fir", Email = "user@gmail.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "user@gmail.com", Password = "Strong!A@#1" },
            new RegisterDTO { Username = "fir_2", Email = "user@gmail.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "dsaads", Password = "000000" },
            new RegisterDTO { Username = "fir", Email = "dsaads", Password = "000000" },
            new RegisterDTO { Username = "fir", Email = "dsaads", Password = "000000" },
            new RegisterDTO { Username = "fir", Email = "dsaads", Password = "000000" }
        };
    }

    public IEnumerable<RegisterDTO> Invalid
    {
        get => new List<RegisterDTO>
        {
            new RegisterDTO { Username = "fi r", Email = "user@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "abc def@gmail.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "dsaads", Password = "asdsda StrongPass1" },

            new RegisterDTO { Username = "fi", Email = "user@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fi", Email = "user@email.com", Password = "Strong1" },

            new RegisterDTO { Username = "fir", Email = "user@email.com", Password = "StrongPasss" },
            new RegisterDTO { Username = "fir", Email = "user@email.com", Password = "strongpass1" },

            new RegisterDTO { Username = "fir👍", Email = "user@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "firć", Email = "user@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "user👍@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "userć@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "user@email.com", Password = "Strongpass1👍" },
            new RegisterDTO { Username = "fir", Email = "user@email.com", Password = "Strongpass1ć" },

            new RegisterDTO { Username = "fir", Email = "user@email.com", Password = "123456789 123456789 123456789 123456789 123456789 1" },
            new RegisterDTO { Username = "123456789 123456789 123456", Email = "userć@email.com", Password = "StrongPass1" },

            new RegisterDTO { Username = "fir!", Email = "user@email.com", Password = "StrongPass1" },
            new RegisterDTO { Username = "fir", Email = "user!@email.com", Password = "StrongPass1" }
        };
    }
}
