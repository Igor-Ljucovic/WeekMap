using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Services;
using System.Security.Cryptography;
using WeekMap.DTOs;

namespace WebApp.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("api/register")]
        public IActionResult Register([FromBody] RegisterDTO data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_context.Users.Any(u => u.Username == data.Username))
                return BadRequest(new { field = "username", message = "Username already exists." });

            if (_context.Users.Any(u => u.Email.ToLower() == data.Email.ToLower()))
                return BadRequest(new { field = "email", message = "Email is already in use." });

            var user = new User
            {
                Username = data.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(data.Password),
                Email = data.Email,
                IsEmailConfirmed = false,
                EmailConfirmationToken = GenerateSecureToken(32),
                EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token = user.EmailConfirmationToken, userId = user.UserID }, protocol: Request.Scheme);
            
            new EmailConfirmationService().SendConfirmationEmail(user, confirmationLink);

            return Ok(new { message = "User registered successfully. Check your email to confirm." });
        }

        private string GenerateSecureToken(int byteLength = 32)
        {
            byte[] tokenBytes = new byte[byteLength];
            RandomNumberGenerator.Fill(tokenBytes);
            return Convert.ToBase64String(tokenBytes);
        }

        [HttpPost("api/login")]
        public IActionResult Login([FromBody] LoginDTO data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _context.Users.FirstOrDefault(u => u.Username == data.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(data.Password, user.Password))
                return Unauthorized(new { message = "Invalid username or password." });

            //if (!user.IsEmailConfirmed)
            //    return StatusCode(403, new { message = "Please confirm your email before logging in." });

            // it has to be converted to string, because only Int32 is supported in HttpContext, but not Int64 (long)
            HttpContext.Session.SetString("UserID", user.UserID.ToString());
            HttpContext.Session.SetString("Username", user.Username);

            return Ok(new { message = "Login successful!" });
        }

        [HttpPost("api/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("api/confirm-email")]
        public IActionResult ConfirmEmail(string token, int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId &&
                                                     u.EmailConfirmationToken == token &&
                                                     u.EmailConfirmationTokenExpiresAt > DateTime.UtcNow);

            if (user.EmailConfirmationTokenExpiresAt < DateTime.UtcNow)
                return BadRequest(new { message = "This confirmation link is invalid or expired." });

            if (user == null)
                return NotFound();

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiresAt = null;
            _context.SaveChanges();

            return Ok(new { message = "Your email has been verified!" });
        }
    }
}