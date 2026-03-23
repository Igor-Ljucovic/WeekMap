using Microsoft.AspNetCore.Mvc;
using WeekMap.DTOs;
using WeekMap.Services.UserSettings;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUserSettingsService _service;

        public UserSettingsController(IUserSettingsService service)
        {
            _service = service;
        }

        private bool TryGetUserId(out long userId)
        {
            userId = 0;
            return long.TryParse(HttpContext.Session.GetString("UserID"), out userId);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByUserId(long id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (id != userId)
                return Unauthorized(new { message = "You can only access your own settings." });

            var settings = await _service.GetByUserIdAsync(userId);
            if (settings == null)
                return NotFound(new { message = "Settings not found." });

            return Ok(settings);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UserSettingsDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != userId)
                return Unauthorized(new { message = "You can only edit your own settings." });

            var ok = await _service.UpdateAsync(userId, dto);
            if (!ok)
                return NotFound(new { message = "Settings not found." });

            return Ok(new { message = "Settings updated successfully!" });
        }
    }
}
