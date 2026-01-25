using Microsoft.AspNetCore.Mvc;
using WeekMap.DTOs;
using WeekMap.Services.User;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        private bool TryGetUserId(out long userId)
        {
            userId = 0;
            return long.TryParse(HttpContext.Session.GetString("UserID"), out userId);
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.TryGetValue("UserID", out _);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.RegisterAsync(dto);

            if (!result.ok)
            {
                // your old controller returned {field, message} for duplicates
                // service returns only message, so we preserve field hints here
                if (result.message.Contains("Username", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { field = "username", message = result.message });

                if (result.message.Contains("Email", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { field = "email", message = result.message });

                return BadRequest(new { message = result.message });
            }

            return Ok(new { message = result.message, id = result.userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.LoginAsync(dto);

            if (!result.ok || result.userId == null || string.IsNullOrWhiteSpace(result.username))
                return Unauthorized(new { message = result.message });

            // session behavior stays the same
            HttpContext.Session.SetString("UserID", result.userId.Value.ToString());
            HttpContext.Session.SetString("Username", result.username);

            return Ok(new
            {
                message = result.message,
                access_token = result.accessToken,
                user = new
                {
                    Username = result.username,
                    UserID = result.userId
                }
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] long userId)
        {
            var result = await _service.ConfirmEmailAsync(token, userId);

            if (!result.ok)
                return BadRequest(new { message = result.message });

            return Ok(new { message = result.message });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAll()
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("users/{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            var user = await _service.GetByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found." });

            return Ok(user);
        }

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO dto)
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(dto);

            if (!result.ok)
            {
                if (result.message.Contains("Username", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { field = "username", message = result.message });

                if (result.message.Contains("Email", StringComparison.OrdinalIgnoreCase))
                    return BadRequest(new { field = "email", message = result.message });

                return BadRequest(new { message = result.message });
            }

            return Ok(new { message = result.message, id = result.userId });
        }

        [HttpPut("users/{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] UserDTO dto)
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);
            if (!result.ok)
                return NotFound(new { message = result.message });

            return Ok(new { message = result.message });
        }

        [HttpDelete("users/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            var result = await _service.DeleteAsync(id);
            if (!result.ok)
                return NotFound(new { message = result.message });

            return Ok(new { message = result.message });
        }
    }
}
