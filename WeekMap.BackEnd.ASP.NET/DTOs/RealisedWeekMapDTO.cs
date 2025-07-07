using System.ComponentModel.DataAnnotations;

namespace WeekMap.DTOs
{
    public class RealisedWeekMapDTO
    {
        public long RealisedWeekMapID { get; set; }

        [Required]
        public long PlannedWeekMapID { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}