using Microsoft.AspNetCore.Mvc;
using WeekMap.DTOs;
using WeekMap.Services.WeekMapActivity;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeekMapActivityController : ControllerBase
    {
        private readonly IWeekMapActivityService _service;

        public WeekMapActivityController(IWeekMapActivityService service)
        {
            _service = service;
        }

        private bool TryGetUserId(out long userId)
        {
            userId = 0;
            return long.TryParse(HttpContext.Session.GetString("UserID"), out userId);
        }

        [HttpGet("{weekMapActivityID:long}")]
        public async Task<IActionResult> GetById(long weekMapActivityID)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var dto = await _service.GetByIdAsync(userId, weekMapActivityID);
            if (dto == null)
                return NotFound(new { message = "Activity not found." });

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] WeekMapActivityDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(userId, dto);

            if (!result.ok)
            {
                if (result.errorMessage != null &&
                    (result.errorMessage.Contains("outside", StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest(new { message = result.errorMessage });
                }

                if (result.errorMessage != null &&
                    (result.errorMessage.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase)))
                {
                    return Unauthorized(new { message = result.errorMessage });
                }

                return NotFound(new { message = result.errorMessage ?? "Create failed." });
            }

            return Ok(new { message = "Activity added successfully!", id = result.id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] WeekMapActivityDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(userId, id, dto);

            if (!result.ok)
            {
                if (result.errorMessage != null &&
                    result.errorMessage.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized(new { message = result.errorMessage });
                }

                if (result.errorMessage != null &&
                    result.errorMessage.Contains("outside", StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new { message = result.errorMessage });
                }

                return NotFound(new { message = result.errorMessage ?? "Activity not found." });
            }

            return Ok(new { message = "Activity updated successfully!" });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var result = await _service.DeleteAsync(userId, id);

            if (!result.ok)
            {
                if (result.errorMessage != null &&
                    result.errorMessage.Contains("Unauthorized", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized(new { message = result.errorMessage });
                }

                return NotFound(new { message = result.errorMessage ?? "Activity not found." });
            }

            return Ok(new { message = "Activity deleted successfully!" });
        }
    }
}
