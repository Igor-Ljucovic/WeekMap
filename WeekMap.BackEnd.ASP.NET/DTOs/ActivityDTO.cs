using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WeekMap.Models;

namespace WeekMap.DTOs
{
    public class ActivityDTO
    {
        public long ActivityID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        [StringLength(50, ErrorMessage = "Location cannot be longer than 50 characters.")]
        public string? Location { get; set; }

        public long UserID { get; set; }

        public long? ActivityCategoryID { get; set; }
    }
}
