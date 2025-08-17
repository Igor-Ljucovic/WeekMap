using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.Models;
using WeekMap.Services;
using System.Security.Cryptography;
using WeekMap.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public UserController(AppDbContext context, IConfiguration configuration, IWebHostEnvironment env)
        {
            _context = context;
            _configuration = configuration;
            _env = env;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO data)
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

            // Add default UserSettings after user is saved
            var defaultUserSettings = new UserSettings { User = user };
            _context.UserSettings.Add(defaultUserSettings);

            // Add UserDefaultWeekMapSettings after user is saved
            var userDefaultWeekMapSettings = new UserDefaultWeekMapSettings { User = user };
            _context.UserDefaultWeekMapSettings.Add(userDefaultWeekMapSettings);

            _context.SaveChanges();

            //var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token = user.EmailConfirmationToken, userId = user.UserID }, protocol: Request.Scheme);

            //new EmailConfirmationService().SendConfirmationEmail(user, confirmationLink);

            return Ok(new { message = "User registered successfully." });
        }

        private string GenerateSecureToken(int byteLength = 32)
        {
            byte[] tokenBytes = new byte[byteLength];
            RandomNumberGenerator.Fill(tokenBytes);
            return Convert.ToBase64String(tokenBytes);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JwtSettings:SecretKey"]; // this gets the key from appsettings.config
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _context.Users.FirstOrDefault(u => u.Email == data.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(data.Password, user.Password))
                return Unauthorized(new { message = "Invalid username or password." });

            //if (!user.IsEmailConfirmed)
            //    return StatusCode(403, new { message = "Please confirm your email before logging in." });

            // it has to be converted to a string, because only Int32 is supported in HttpContext, but not Int64 (long)
            HttpContext.Session.SetString("UserID", user.UserID.ToString());
            HttpContext.Session.SetString("Username", user.Username);

            return Ok(new
            {
                message = "Login successful!",
                access_token = GenerateJwtToken(user),
                user = new
                {
                    Username = user.Username,
                    UserID = user.UserID
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (!HttpContext.Session.TryGetValue("UserID", out _))
                return Unauthorized(new { message = "User not logged in." });

            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("confirm-email")]
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

        [HttpGet("users")]
        public IActionResult GetAll()
        {
            if (!HttpContext.Session.TryGetValue("UserID", out _))
                return Unauthorized(new { message = "User not logged in." });

            var users = _context.Users
                .Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.Email,
                    u.IsEmailConfirmed
                })
                .ToList();

            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public IActionResult GetById(long id)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out _))
                return Unauthorized(new { message = "User not logged in." });

            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.UserID,
                user.Username,
                user.Email,
                user.IsEmailConfirmed
            });
        }

        [HttpPost("users")]
        public IActionResult CreateUser([FromBody] UserDTO data)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out _))
                return Unauthorized(new { message = "User not logged in." });

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

            return Ok(new { message = "User created successfully." });
        }

        [HttpPut("users/{id}")]
        public IActionResult Edit(long id, [FromBody] UserDTO data)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out _))
                return Unauthorized(new { message = "User not logged in." });

            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            user.Username = data.Username;
            user.Email = data.Email;

            if (!string.IsNullOrWhiteSpace(data.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);

            _context.SaveChanges();

            return Ok(new { message = "User updated successfully." });
        }

        [HttpDelete("users/{id}")]
        public IActionResult Delete(long id)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out _))
                return Unauthorized(new { message = "User not logged in." });

            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return NotFound();

            _context.Users.Remove(user); // Automatically cascades to UserSettings
            _context.SaveChanges();

            return Ok(new { message = "User deleted successfully." });
        }
    }
}