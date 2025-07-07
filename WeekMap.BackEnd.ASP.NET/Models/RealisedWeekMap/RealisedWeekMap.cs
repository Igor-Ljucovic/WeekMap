using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WeekMap.Models
{
    public class RealisedWeekMap
    {
        public long RealisedWeekMapID { get; set; }

        public long PlannedWeekMapID { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [ValidateNever]
        public PlannedWeekMap PlannedWeekMap { get; set; }
    }
}