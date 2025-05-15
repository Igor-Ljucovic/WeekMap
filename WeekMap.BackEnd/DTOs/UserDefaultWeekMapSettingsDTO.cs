using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.DTOs
{
    public class UserDefaultWeekMapSettingsDTO
    {
        public long UserID { get; set; }

        [Required(ErrorMessage = "Skip sunday is required.")]
        public bool SkipSaturday { get; set; } = false;
        [Required(ErrorMessage = "Skip sunday is required.")]
        public bool SkipSunday { get; set; } = false;

        [Required(ErrorMessage = "Week start day is required.")]
        [ValidWeekDay]
        public string WeekStartDay { get; set; } = "Monday";

        [Required(ErrorMessage = "Day start time is required.")]
        public TimeSpan DayStartTime { get; set; } = new TimeSpan(0, 0, 0);

        [Required(ErrorMessage = "Day end time is required.")]
        public TimeSpan DayEndTime { get; set; } = new TimeSpan(23, 59, 59);
        [Required(ErrorMessage = "Show place in preview is required.")]
        public bool ShowPlaceInPreview { get; set; } = false;
        [Required(ErrorMessage = "Show description in preview is required.")]
        public bool ShowDescriptionInPreview { get; set; } = false;
    }
}

