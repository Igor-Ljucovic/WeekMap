using Microsoft.AspNetCore.Mvc;
using WeekMap.DTOs;
using WeekMap.Services.WeekMap;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeekMapController : BaseApiController
    {
        private readonly IWeekMapService _service;

        public WeekMapController(IWeekMapService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var maps = await _service.GetAllAsync(userId);
            return Ok(maps);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetLatestForUser(long id)
        {
            if (!TryGetUserId(out var sessionUserId))
                return Unauthorized(new { message = "User not logged in." });

            if (sessionUserId != id)
                return Unauthorized(new { message = "Unauthorized access." });

            var map = await _service.GetLatestByUserIdAsync(sessionUserId, id);

            if (map == null)
                return NotFound(new { message = "No week maps found for user." });

            return Ok(map);
        }

        [HttpGet("{id:long}/activityTemplates")]
        public async Task<IActionResult> GetWeekMapActivities(long id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var activities = await _service.GetWeekMapActivitiesAsync(userId, id);

            if (activities.Count == 0)
                return NotFound(new { message = "Week map not found (or you don't have access)." });

            return Ok(activities);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WeekMapDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(userId, dto);

            return Ok(new { message = "Week map added successfully!", id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] WeekMapDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _service.UpdateAsync(userId, id, dto);
            if (!ok)
                return NotFound(new { message = "Week map not found." });

            return Ok(new { message = "Week map updated successfully!" });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var ok = await _service.DeleteAsync(userId, id);
            if (!ok)
                return NotFound(new { message = "Week map not found." });

            return Ok(new { message = "Week map deleted successfully!" });
        }
    }
}
