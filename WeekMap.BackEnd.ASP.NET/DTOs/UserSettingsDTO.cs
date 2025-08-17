using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.DTOs
{
    public class UserSettingsDTO
    {
        public long UserID { get; set; }

        [ThemeValidation(ErrorMessage = "Theme must be either 'light' or 'dark'.")]
        public string Theme { get; set; } = "light";
    }
}