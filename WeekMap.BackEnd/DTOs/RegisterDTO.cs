using System.ComponentModel.DataAnnotations;

namespace WeekMap.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3)]
        [MaxLength(25)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
