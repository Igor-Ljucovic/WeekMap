using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class PlannedWeekMap
    {
        public long PlannedWeekMapID { get; set; }

        public bool ShowSaturday { get; set; }
        public bool ShowSaturdayAndSunday { get; set; }

        [Required(ErrorMessage = "Week start day is required.")]
        [StringLength(10, ErrorMessage = "Week start day cannot be longer than 10 characters.")]
        public string WeekStartDay { get; set; }

        [Required(ErrorMessage = "Day start time is required.")]
        public TimeSpan DayStartTime { get; set; }

        [Required(ErrorMessage = "Day end time is required.")]
        public TimeSpan DayEndTime { get; set; }

        public bool ShowPlaceInPreview { get; set; }
        public bool ShowDescriptionInPreview { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public long UserID { get; set; }

        [ValidateNever]
        public User User { get; set; }
        [ValidateNever]
        public ICollection<RealisedWeekMap> RealisedWeekMaps { get; set; } = new List<RealisedWeekMap>();
    }
}
