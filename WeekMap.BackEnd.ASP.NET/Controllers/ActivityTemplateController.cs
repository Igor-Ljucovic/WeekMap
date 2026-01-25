using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;
using WeekMap.Services.ActivityTemplate;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityTemplateController : ControllerBase
    {
        private readonly IActivityTemplateService _service;

        public ActivityTemplateController(IActivityTemplateService service)
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

            var templates = await _service.GetAllAsync(userId);
            return Ok(templates);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ActivityTemplateDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _service.CreateAsync(userId, dto);

            return Ok(new { message = "Activity template added successfully!", id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit(long id, [FromBody] ActivityTemplateDTO dto)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(userId, id, dto);
            if (!updated)
                return NotFound(new { message = "Activity template not found." });

            return Ok(new { message = "Activity template updated successfully!" });
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            if (!TryGetUserId(out var userId))
                return Unauthorized(new { message = "User not logged in." });

            var deleted = await _service.DeleteAsync(userId, id);
            if (!deleted)
                return NotFound(new { message = "Activity template not found." });

            return Ok(new { message = "Activity template deleted successfully!" });
        }
    }
}

