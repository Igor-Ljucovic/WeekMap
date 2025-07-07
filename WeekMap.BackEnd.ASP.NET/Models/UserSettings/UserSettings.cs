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

        public bool Notification { get; set; } = false;

        [ValidNotificationTime(ErrorMessage = "Time must be positive and less than 23:00:00.")]
        [Required(ErrorMessage = "Notification time is required.")]
        public TimeSpan NotificationTime { get; set; } = new TimeSpan(21, 0, 0);

        public bool EmailUpdates { get; set; } = false;
    }
}
