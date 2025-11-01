using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    [ValidActivityTimeRange]
    public class WeekMapActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long WeekMapActivityID { get; set; }

        [ValidateNever]
        public WeekMap WeekMap { get; set; }
        public long WeekMapID { get; set; }
        [ValidateNever]
        public ActivityTemplate ActivityTemplate { get; set; }
        public long ActivityTemplateID { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan EndTime { get; set; }
        public bool RepeatEveryWeek { get; set; } = true;
        public DateTime? ActivityDate { get; set; }

        public bool OnMonday { get; set; }
        public bool OnTuesday { get; set; }
        public bool OnWednesday { get; set; }
        public bool OnThursday { get; set; }
        public bool OnFriday { get; set; }
        public bool OnSaturday { get; set; }
        public bool OnSunday { get; set; }
    }
}

