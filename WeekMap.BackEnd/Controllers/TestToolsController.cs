using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/test-tools")]
    public class TestToolsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TestToolsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpDelete("cleanup-all")]
        public IActionResult CleanupAllTables()
        {
            if (!_env.IsEnvironment("Test"))
                return Forbid("CleanupAllTables can only run in the Test environment.");
            

            // the order MATTERS
            _context.TaskCategories.RemoveRange(_context.TaskCategories);
            _context.RepetitiveTasks.RemoveRange(_context.RepetitiveTasks);
            _context.CalendarTasks.RemoveRange(_context.CalendarTasks);
            _context.NoDeadlineTasks.RemoveRange(_context.NoDeadlineTasks);
            _context.UserTasks.RemoveRange(_context.UserTasks);
            _context.Users.RemoveRange(_context.Users);

            _context.SaveChanges();

            return Ok(new { message = "All data wiped from database." });
        }
    }
}
