using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserSettings
    {
        [Key]
        public long UserID { get; set; }

        public User User { get; set; }

        [StringLength(30, ErrorMessage = "Theme cannot be longer than 30 characters.")]
        public string Theme { get; set; } = "light";

        public bool Notification { get; set; } = false;

        [Required(ErrorMessage = "Notification time is required.")]
        public TimeSpan NotificationTime { get; set; } = TimeSpan.Zero;

        public bool EmailUpdates { get; set; } = false;
    }
}
