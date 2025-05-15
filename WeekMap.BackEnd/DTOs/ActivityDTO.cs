using System.ComponentModel.DataAnnotations;

namespace WeekMap.DTOs
{
    public class ActivityDTO
    {
        public long ActivityID { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public long? ActivityCategoryID { get; set; }

        [Required]
        public long UserID { get; set; }
    }
}
