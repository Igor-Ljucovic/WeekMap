using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    [ValidWeekMapTimeRange(ErrorMessage = "Day start time must be before or equal to day end time.")]
    public class WeekMap
    {
        public long WeekMapID { get; set; }
        
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must be at most 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Day start time is required.")]
        public TimeSpan DayStartTime { get; set; }

        [Required(ErrorMessage = "Day end time is required.")]
        public TimeSpan DayEndTime { get; set; }
        [Required(ErrorMessage = "Show place in preview is required.")]
        public bool ShowPlaceInPreview { get; set; }
        [Required(ErrorMessage = "Show description in preview is required.")]
        public bool ShowDescriptionInPreview { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public long UserID { get; set; }

        [ValidateNever]
        public User User { get; set; }
    }
}
