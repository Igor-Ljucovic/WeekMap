using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    [ValidWeekMapTimeRange(ErrorMessage = "Day start time must be before or equal to day end time.")]
    public class PlannedWeekMap
    {
        public long PlannedWeekMapID { get; set; }
        [Required(ErrorMessage = "Show Saturday is required.")]
        public bool ShowSaturday { get; set; }
        [Required(ErrorMessage = "Show Sunday is required.")]
        public bool ShowSunday { get; set; }

        [Required(ErrorMessage = "Week start day is required.")]
        [ValidWeekDay]
        public string WeekStartDay { get; set; }

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
        [ValidateNever]
        public ICollection<RealisedWeekMap> RealisedWeekMaps { get; set; } = new List<RealisedWeekMap>();
    }
}
