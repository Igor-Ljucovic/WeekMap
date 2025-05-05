using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class UserDefaultWeekMapSettings
    {
        [Key]
        public long UserID { get; set; } 

        public User User { get; set; }

        public bool SkipSaturday { get; set; } = false;
        public bool SkipSunday { get; set; } = false;

        [Required(ErrorMessage = "Week start day is required.")]
        [StringLength(10, ErrorMessage = "Week start day cannot be longer than 10 characters.")]
        public string WeekStartDay { get; set; } = "monday";

        [Required(ErrorMessage = "Day start time is required.")]
        public TimeSpan DayStartTime { get; set; } = TimeSpan.Zero;

        [Required(ErrorMessage = "Day end time is required.")]
        public TimeSpan DayEndTime { get; set; } = TimeSpan.Zero;

        public bool ShowPlaceInPreview { get; set; } = false;
        public bool ShowDescriptionInPreview { get; set; } = false;
    }
}
