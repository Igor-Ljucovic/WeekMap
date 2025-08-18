using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    [ValidWeekMapTimeRange(ErrorMessage = "Day start time must be before or equal to day end time.")]
    public class UserDefaultWeekMapSettings
    {
        [Key]
        public long UserID { get; set; } 

        public User? User { get; set; }

        [Required(ErrorMessage = "Day start time is required.")]
        public TimeSpan DayStartTime { get; set; } = new TimeSpan(0, 0, 0);

        [Required(ErrorMessage = "Day end time is required.")]
        public TimeSpan DayEndTime { get; set; } = new TimeSpan(23, 59, 59);
        [Required(ErrorMessage = "Show place in preview is required.")]
        public bool ShowLocationInPreview { get; set; } = false;
        [Required(ErrorMessage = "Show description in preview is required.")]
        public bool ShowDescriptionInPreview { get; set; } = false;
    }
}
