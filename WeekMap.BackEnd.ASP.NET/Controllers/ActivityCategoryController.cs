using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityCategoryController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var categories = _context.ActivityCategories.Where(c => c.UserID == userId).ToList();

            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ActivityCategoryDTO category)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            category.UserID = userId;

            var entity = _mapper.Map<ActivityCategory>(category);

            _context.ActivityCategories.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "Category added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] ActivityCategoryDTO updatedCategory)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var category = _context.ActivityCategories.FirstOrDefault(c => c.ActivityCategoryID == id);
            if (category == null)
                return NotFound();

            _mapper.Map(updatedCategory, category);

            _context.SaveChanges();

            return Ok(new { message = "Category updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var category = _context.ActivityCategories.FirstOrDefault(c => c.ActivityCategoryID == id);
            if (category == null)
                return NotFound();

            _context.ActivityCategories.Remove(category);
            _context.SaveChanges();

            return Ok(new { message = "Category deleted successfully!" });
        }
    }
}
