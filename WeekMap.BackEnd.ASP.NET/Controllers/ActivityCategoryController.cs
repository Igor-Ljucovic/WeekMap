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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var categories = await _service.GetAllAsync(userId);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ActivityCategoryDTO category)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(userId, category);

            return Ok(new { message = "Category added successfully!", activityCategoryId = id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] ActivityCategoryDTO updatedCategory)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(userId, id, updatedCategory);
            if (!updated) return NotFound();

            return Ok(new { message = "Category updated successfully!" });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var deleted = await _service.DeleteAsync(userId, id);
            if (!deleted) return NotFound();

            return Ok(new { message = "Category deleted successfully!" });
        }
    }
}