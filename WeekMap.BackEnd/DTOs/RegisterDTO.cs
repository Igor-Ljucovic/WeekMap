using System.ComponentModel.DataAnnotations;

namespace WeekMap.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters long.")]
        [StringLength(25, ErrorMessage = "Username cannot be longer than 25 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_]+$", ErrorMessage = "Username can only contain alphabet letters, numbers or _")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [StringLength(64, ErrorMessage = "Password cannot be longer than 64 characters.")]
        [StrongPassword]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
    }
}
