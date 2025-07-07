using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    [ValidActivityTimeRange]
    public class RealisedWeekMapActivity
    {
        [Key]
        public long RealisedWeekMapActivityID { get; set; }

        [ValidateNever]
        public Activity Activity { get; set; }
        public long ActivityID { get; set; }
        [ValidateNever]
        public RealisedWeekMap RealisedWeekMap { get; set; }
        public long RealisedWeekMapID { get; set; }

        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Realised start time is required.")]
        public TimeSpan RealisedStartTime { get; set; }

        [Required(ErrorMessage = "Realised end time is required.")]
        public TimeSpan RealisedEndTime { get; set; }
    }
}