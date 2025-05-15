using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    [ValidActivityTimeRange]
    public class PlannedWeekMapActivity
    {
        [Key]
        public long PlannedWeekMapActivityID { get; set; }

        [ValidateNever]
        public PlannedWeekMap PlannedWeekMap { get; set; }
        public long PlannedWeekMapID { get; set; }
        [ValidateNever]
        public Activity Activity { get; set; }
        public long ActivityID { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan EndTime { get; set; }

        public bool OnMonday { get; set; }
        public bool OnTuesday { get; set; }
        public bool OnWednesday { get; set; }
        public bool OnThursday { get; set; }
        public bool OnFriday { get; set; }
        public bool OnSaturday { get; set; }
        public bool OnSunday { get; set; }
    }
}