using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;
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
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var categories = await _service.GetAllAsync(userId);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ActivityCategoryDTO category)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateAsync(userId, category);
            return Ok(new { message = "Category added successfully!" });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] ActivityCategoryDTO updatedCategory)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var ok = await _service.UpdateAsync(userId, id, updatedCategory);
            return ok ? Ok(new { message = "Category updated successfully!" }) : NotFound();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var ok = await _service.DeleteAsync(userId, id);
            return ok ? Ok(new { message = "Category deleted successfully!" }) : NotFound();
        }
    }
}
