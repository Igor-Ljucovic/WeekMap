using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace XUnitTests
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActivityCategoryController(AppDbContext context)
        {
            _context = context;
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
        public IActionResult Add([FromBody] ActivityCategory category)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            category.UserID = userId;
            _context.ActivityCategories.Add(category);
            _context.SaveChanges();

            return Ok(new { message = "Category added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] ActivityCategory updatedCategory)
        {
            var category = _context.ActivityCategories.FirstOrDefault(c => c.ActivityCategoryID == id);
            if (category == null)
                return NotFound();

            category.Name = updatedCategory.Name;
            category.Color = updatedCategory.Color;
            _context.SaveChanges();

            return Ok(new { message = "Category updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.ActivityCategories.FirstOrDefault(c => c.ActivityCategoryID == id);
            if (category == null)
                return NotFound();

            _context.ActivityCategories.Remove(category);
            _context.SaveChanges();

            return Ok(new { message = "Category deleted successfully!" });
        }
    }
}
