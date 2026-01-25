using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;

namespace WeekMap.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<(bool ok, string message, long? userId)> RegisterAsync(UserDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return (false, "Username already exists.", null);

            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return (false, "Email is already in use.", null);

            var user = new Models.User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Email = dto.Email,
                IsEmailConfirmed = false,
                EmailConfirmationToken = GenerateSecureToken(32),
                EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            _context.Users.Add(user);

            _context.UserSettings.Add(new Models.UserSettings { User = user });
            _context.UserDefaultWeekMapSettings.Add(new Models.UserDefaultWeekMapSettings { User = user });

            await _context.SaveChangesAsync();

            return (true, "User registered successfully.", user.UserID);
        }

        public async Task<(bool ok, string message, string? accessToken, long? userId, string? username)> LoginAsync(LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                return (false, "Invalid username or password.", null, null, null);

            // if (!user.IsEmailConfirmed)
            //     return (false, "Please confirm your email before logging in.", null, null, null);

            var token = GenerateJwtToken(user);

            return (true, "Login successful!", token, user.UserID, user.Username);
        }

        public async Task<(bool ok, string message)> ConfirmEmailAsync(string token, long userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserID == userId &&
                u.EmailConfirmationToken == token);

            if (user == null)
                return (false, "User not found or invalid token.");

            if (user.EmailConfirmationTokenExpiresAt == null || user.EmailConfirmationTokenExpiresAt <= DateTime.UtcNow)
                return (false, "This confirmation link is invalid or expired.");

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiresAt = null;

            await _context.SaveChangesAsync();
            return (true, "Your email has been verified!");
        }

        public async Task<List<object>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => (object)new
                {
                    u.UserID,
                    u.Username,
                    u.Email,
                    u.IsEmailConfirmed
                })
                .ToListAsync();
        }

        public async Task<object?> GetByIdAsync(long id)
        {
            var user = await _context.Users
                .Where(u => u.UserID == id)
                .Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.Email,
                    u.IsEmailConfirmed
                })
                .FirstOrDefaultAsync();

            return user == null ? null : (object)user;
        }

        public async Task<(bool ok, string message, long? userId)> CreateAsync(UserDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return (false, "Username already exists.", null);

            if (await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return (false, "Email is already in use.", null);

            var user = new Models.User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Email = dto.Email,
                IsEmailConfirmed = false,
                EmailConfirmationToken = GenerateSecureToken(32),
                EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "User created successfully.", user.UserID);
        }

        public async Task<(bool ok, string message)> UpdateAsync(long id, UserDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
                return (false, "User not found.");

            user.Username = dto.Username;
            user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.SaveChangesAsync();
            return (true, "User updated successfully.");
        }

        public async Task<(bool ok, string message)> DeleteAsync(long id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user == null)
                return (false, "User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return (true, "User deleted successfully.");
        }

        private static string GenerateSecureToken(int byteLength = 32)
        {
            byte[] tokenBytes = new byte[byteLength];
            RandomNumberGenerator.Fill(tokenBytes);
            return Convert.ToBase64String(tokenBytes);
        }

        private string GenerateJwtToken(Models.User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JwtSettings:SecretKey"];

            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JwtSettings:SecretKey is not configured.");

            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
