using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskCategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TaskCategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return Unauthorized(new { message = "User not logged in." });

            var categories = _context.TaskCategories.Where(c => c.UserID == userId).ToList();

            return Ok(categories);
        }

        [HttpPost]
        public IActionResult Add([FromBody] TaskCategory category)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return Unauthorized(new { message = "User not logged in." });

            category.UserID = userId.Value;
            _context.TaskCategories.Add(category);
            _context.SaveChanges();

            return Ok(new { message = "Category added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] TaskCategory updatedCategory)
        {
            var category = _context.TaskCategories.FirstOrDefault(c => c.TaskCategoryID == id);
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
            var category = _context.TaskCategories.FirstOrDefault(c => c.TaskCategoryID == id);
            if (category == null)
                return NotFound();

            _context.TaskCategories.Remove(category);
            _context.SaveChanges();

            return Ok(new { message = "Category deleted successfully!" });
        }
    }
}
