using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    public class UserSettings
    {
        [Key]
        public long UserID { get; set; }

        public User? User { get; set; }

        [ThemeValidation(ErrorMessage = "Theme must be either 'light' or 'dark'.")]
        public string Theme { get; set; } = "light";
    }
}
