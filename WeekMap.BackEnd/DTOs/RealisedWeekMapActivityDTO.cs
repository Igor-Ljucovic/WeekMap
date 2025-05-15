using System.ComponentModel.DataAnnotations;
using WeekMap.Attributes;

namespace WeekMap.Models
{
    [ValidActivityTimeRange]
    public class RealisedWeekMapActivityDTO
    {
        public long RealisedWeekMapActivityID { get; set; }
        public long ActivityID { get; set; }
        public long RealisedWeekMapID { get; set; }
        public bool IsCompleted { get; set; }

        [Required(ErrorMessage = "Realised start time is required.")]
        public TimeSpan RealisedStartTime { get; set; }

        [Required(ErrorMessage = "Realised end time is required.")]
        public TimeSpan RealisedEndTime { get; set; }
    }
}