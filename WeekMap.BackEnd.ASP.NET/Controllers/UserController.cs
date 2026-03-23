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

            if (!result.ok || result.userId == null)
                return BadRequest(new { message = "Username or email already exists." });

            return Ok(new { message = "Registration successful", id = result.userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.LoginAsync(dto);

            if (!result.ok || result.userId == null || string.IsNullOrWhiteSpace(result.username))
                return Unauthorized(new { message = "Log in failed" });

            HttpContext.Session.SetString("UserID", result.userId.Value.ToString());
            HttpContext.Session.SetString("Username", result.username);

            return Ok(new
            {
                message = "Log in successful",
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

            if (!result)
                return BadRequest();

            return Ok();
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

        [HttpPut("users/{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] UserDTO dto)
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(id, dto);
            if (!result)
                return NotFound(new { message = "User not found." });

            return Ok(new { message = "User successfully updated." });
        }

        [HttpDelete("users/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!IsLoggedIn())
                return Unauthorized(new { message = "User not logged in." });

            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "User not found." });

            return Ok(new { message = "User successfully deleted." });
        }
    }
}
