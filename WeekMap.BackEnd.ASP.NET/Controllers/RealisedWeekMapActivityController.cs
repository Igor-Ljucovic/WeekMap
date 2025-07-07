using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.Models;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RealisedWeekMapActivityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RealisedWeekMapActivityController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{realisedWeekMapID}/{activityID}")]
        public IActionResult GetById(long realisedWeekMapID, long activityID)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.RealisedWeekMapActivities.FirstOrDefault(pwma => pwma.RealisedWeekMapID == realisedWeekMapID && pwma.ActivityID == activityID);

            if (activity == null)
                return NotFound();

            return Ok(activity);
        }

        [HttpPost]
        public IActionResult Add([FromBody] RealisedWeekMapActivityDTO realisedWeekMapActivity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the associated Activity exists
            var existingActivity = _context.Activities.FirstOrDefault(a => a.ActivityID == realisedWeekMapActivity.ActivityID);
            if (existingActivity == null)
                return NotFound(new { message = $"Activity with ID {realisedWeekMapActivity.ActivityID} does not exist." });

            // Check if the associated RealisedWeekMap exists
            var existingMap = _context.RealisedWeekMaps.FirstOrDefault(p => p.RealisedWeekMapID == realisedWeekMapActivity.RealisedWeekMapID);
            if (existingMap == null)
                return NotFound(new { message = $"RealisedWeekMap with ID {realisedWeekMapActivity.RealisedWeekMapID} does not exist." });

            var entity = _mapper.Map<RealisedWeekMapActivity>(realisedWeekMapActivity);

            _context.RealisedWeekMapActivities.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "RealisedWeekMapActivity added successfully!" });
        }

        [HttpPut("{realisedWeekMapID}/{activityID}")]
        public IActionResult Edit(long realisedWeekMapID, long activityID, [FromBody] RealisedWeekMapActivityDTO updatedRealisedWeekMapActivity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var realisedWeekMapActivity = _context.RealisedWeekMapActivities.FirstOrDefault(rwma => rwma.RealisedWeekMapID == realisedWeekMapID && rwma.ActivityID == activityID);

            if (realisedWeekMapActivity == null)
                return NotFound();

            realisedWeekMapActivity.RealisedStartTime = updatedRealisedWeekMapActivity.RealisedStartTime;
            realisedWeekMapActivity.RealisedEndTime = updatedRealisedWeekMapActivity.RealisedEndTime;
            realisedWeekMapActivity.IsCompleted = updatedRealisedWeekMapActivity.IsCompleted;

            _context.SaveChanges();

            return Ok(new { message = "RealisedWeekMapActivity updated successfully!" });
        }

        [HttpDelete("{realisedWeekMapID}/{activityID}")]
        public IActionResult Delete(long realisedWeekMapID, long activityID)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var realisedWeekMapActivity = _context.RealisedWeekMapActivities.FirstOrDefault(pwma => pwma.RealisedWeekMapID == realisedWeekMapID && pwma.ActivityID == activityID);

            if (realisedWeekMapActivity == null)
                return NotFound();

            _context.RealisedWeekMapActivities.Remove(realisedWeekMapActivity);
            _context.SaveChanges();

            return Ok(new { message = "RealisedWeekMapActivity deleted successfully!" });
        }
    }
}
