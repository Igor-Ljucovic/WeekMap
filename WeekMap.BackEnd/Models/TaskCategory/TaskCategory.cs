using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class TaskCategory
    {
        public int TaskCategoryID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Color is required.")]
        public string? Color { get; set; }

        public int UserID { get; set; }
        [ValidateNever]
        public User User { get; set; }
    }
}
