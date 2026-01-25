using Microsoft.AspNetCore.Mvc;
using WeekMap.DTOs;
using WeekMap.Services.ActivityCategory;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityCategoryController : ControllerBase
    {
        private readonly IActivityCategoryService _service;

        public ActivityCategoryController(IActivityCategoryService service)
        {
            _service = service;
        }

        private bool TryGetUserId(out long userId)
        {
            userId = 0;
            return long.TryParse(HttpContext.Session.GetString("UserID"), out userId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var categories = await _service.GetAllAsync(userId);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ActivityCategoryDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(userId, dto);

            return Ok(new { message = "Category added successfully!", id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] ActivityCategoryDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ok = await _service.UpdateAsync(userId, id, dto);
            if (!ok)
                return NotFound(new { message = "Category not found." });

            return Ok(new { message = "Category updated successfully!" });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var ok = await _service.DeleteAsync(userId, id);
            if (!ok)
                return NotFound(new { message = "Category not found." });

            return Ok(new { message = "Category deleted successfully!" });
        }
    }
}
