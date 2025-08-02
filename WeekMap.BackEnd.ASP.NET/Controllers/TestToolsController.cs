using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;

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

            // The order MATTERS - delete child tables first
            _context.RealisedWeekMapActivities.RemoveRange(_context.RealisedWeekMapActivities);
            _context.PlannedWeekMapActivities.RemoveRange(_context.PlannedWeekMapActivities);

            _context.RealisedWeekMaps.RemoveRange(_context.RealisedWeekMaps);
            _context.PlannedWeekMaps.RemoveRange(_context.PlannedWeekMaps);

            _context.Activities.RemoveRange(_context.Activities);
            _context.ActivityCategories.RemoveRange(_context.ActivityCategories);

            _context.UserDefaultWeekMapSettings.RemoveRange(_context.UserDefaultWeekMapSettings);
            _context.UserSettings.RemoveRange(_context.UserSettings);

            _context.Users.RemoveRange(_context.Users);

            _context.SaveChanges();

            return Ok(new { message = "All data wiped from database." });
        }
    }
}
