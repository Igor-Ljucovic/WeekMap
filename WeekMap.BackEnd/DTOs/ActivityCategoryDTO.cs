using System.ComponentModel.DataAnnotations;

namespace WeekMap.DTOs
{
    public class ActivityCategoryDTO
    {
        [Required(ErrorMessage = "ActivityCategoryID is required.")]
        public long ActivityCategoryID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name must be shorter then 51 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Color is required.")]
        [RegularExpression("^[A-Fa-f0-9]{6}$", ErrorMessage = "Color must be a 6-digit hexadecimal (0-9, A-F).")]
        public string Color { get; set; }
        [Required(ErrorMessage = "UserID is required.")]
        public long UserID { get; set; }
    }
}
