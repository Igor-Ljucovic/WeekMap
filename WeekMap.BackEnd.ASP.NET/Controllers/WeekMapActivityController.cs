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
        public async Task<IActionResult> Create([FromBody] WeekMapActivityDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(userId, dto);

            if (result == null)
                return NotFound(new { message = "WeekMap or ActivityTemplate not found." });

            return Ok(new { message = "Activity added successfully!"});
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] WeekMapActivityDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(userId, id, dto);

            return Ok(new { message = "Activity updated successfully!" });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var result = await _service.DeleteAsync(userId, id);

            return Ok(new { message = "Activity deleted successfully!" });
        }
    }
}
