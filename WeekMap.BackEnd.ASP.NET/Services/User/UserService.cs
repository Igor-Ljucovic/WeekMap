using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WeekMap.DTOs;
using WeekMap.Models;
using WeekMap.Repositories.User;

namespace WeekMap.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public async Task<(bool ok, long? userId)> RegisterAsync(UserDTO dto)
        {
            if (await _repo.UsernameExistsAsync(dto.Username))
                return (false, null);

            if (await _repo.EmailExistsAsync(dto.Email.ToLower()))
                return (false, null);

            var user = new Models.User
            {
                Username = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Email = dto.Email,
                IsEmailConfirmed = false,
                EmailConfirmationToken = GenerateSecureToken(32),
                EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddMinutes(10)
            };

            _repo.Create(user);

            var userId = user.UserID;

            _repo.AddUserSettings(new Models.UserSettings { UserID = userId });
            _repo.AddUserDefaultWeekMapSettings(new Models.UserDefaultWeekMapSettings { UserID = userId });

            await _repo.SaveChangesAsync();

            return (true, userId);
        }

        public async Task<(bool ok, string? accessToken, long? userId, string? username)> LoginAsync(LoginDTO dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
               return (false, null, null, null);

            // if (!user.IsEmailConfirmed)
            //     return (false, "Please confirm your email before logging in.", null, null, null);

            var token = GenerateJwtToken(user);

            return (true, token, user.UserID, user.Username);
        }

        public async Task<bool> ConfirmEmailAsync(string token, long userId)
        {
            var user = await _repo.GetByEmailConfirmationTokenAsync(userId, token);

            if (user == null)
                return false;

            if (user.EmailConfirmationTokenExpiresAt == null || user.EmailConfirmationTokenExpiresAt <= DateTime.UtcNow)
                return false;

            user.IsEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiresAt = null;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();

            return users.Select(u => (object)new
            {
                u.UserID,
                u.Username,
                u.Email,
                u.IsEmailConfirmed
            }).ToList();
        }

        public async Task<object?> GetByIdAsync(long id)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null)
                return null;

            return new
            {
                user.UserID,
                user.Username,
                user.Email,
                user.IsEmailConfirmed
            };
        }

        public async Task<bool> UpdateAsync(long id, UserDTO dto)
        {
            var user = await _repo.GetByIdAsync(id);

            user.Username = dto.Username;
            user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var user = await _repo.GetByIdAsync(id);

            _repo.Delete(user);
            await _repo.SaveChangesAsync();

            return true;
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
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}