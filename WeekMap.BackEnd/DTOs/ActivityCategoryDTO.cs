using System.ComponentModel.DataAnnotations;

namespace WeekMap.DTOs
{
    public class ActivityCategoryDTO
    {
        [Required(ErrorMessage = "ActivityCategoryID is required.")]
        public long ActivityCategoryID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Color is required.")]
        public string Color { get; set; }
        [Required(ErrorMessage = "UserID is required.")]
        public long UserID { get; set; }
    }
}
