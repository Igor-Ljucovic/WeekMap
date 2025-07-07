using WeekMap.DTOs;
using XUnitTests.TestData;

public class UserTestData : ITestData<UserDTO>
{
    public IEnumerable<UserDTO> Valid =>
    new List<UserDTO>
    {
        new UserDTO { Username = "fir1", Email = "user1@gmail.com", Password = "StrongPass1" },
        new UserDTO { Username = "fir2", Email = "user2@gmail.com", Password = "Strong!A@#1" },
        new UserDTO { Username = "fir_2", Email = "user3@gmail.com", Password = "StrongPass1" },

        new UserDTO { Username = "fir7", Email = "userć@email.com", Password = "StrongPass2" },
        new UserDTO { Username = "fir8", Email = "user👍@email.com", Password = "StrongPass3" },
    };
    

    public IEnumerable<UserDTO> Invalid =>
    new List<UserDTO>
    {
        new UserDTO { Username = "fi r", Email = "user@email.com", Password = "StrongPass1" },
        new UserDTO { Username = "fir", Email = "dsaads", Password = "asdsda StrongPass1" },

        new UserDTO { Username = "fi", Email = "user@email.com", Password = "StrongPass1" },
        new UserDTO { Username = "fir", Email = "@email.com", Password = "StrongPass1" },
        new UserDTO { Username = "fir", Email = "user@email.com", Password = "Strong1" },

        new UserDTO { Username = "fir", Email = "user@email.com", Password = "StrongPasss" },
        new UserDTO { Username = "fir", Email = "user@email.com", Password = "strongpass1" },

        new UserDTO { Username = "fir👍", Email = "user@email.com", Password = "StrongPass1" },
        new UserDTO { Username = "firć", Email = "user@email.com", Password = "StrongPass1" },

        new UserDTO { Username = "fir", Email = "user@email.com", Password = "Strongpass1👍" },
        new UserDTO { Username = "fir", Email = "user@email.com", Password = "Strongpass1ć" },

        new UserDTO { Username = "fir", Email = "user@email.com", Password = "0123456789012345678991234567890123456789012345678901" },
        new UserDTO { Username = "012345678901234567890123456", Email = "userć@email.com", Password = "StrongPass1" },

        new UserDTO { Username = "fir!", Email = "user@email.com", Password = "StrongPass1" },

        new UserDTO { Email = "user!@email.com", Password = "StrongPass1" },
        new UserDTO { Username = "fir", Password = "StrongPass1" },
        new UserDTO { Username = "fir", Email = "user!@email.com" },
        new UserDTO {  },
    };
}
