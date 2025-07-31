using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.Models;
using WeekMap.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlannedWeekMapActivityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlannedWeekMapActivityController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{plannedWeekMapActivityID}")]
        public IActionResult GetById(long plannedWeekMapActivityID)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.PlannedWeekMapActivities
                .FirstOrDefault(pwma => pwma.PlannedWeekMapActivityID == plannedWeekMapActivityID);

            if (activity == null)
                return NotFound();

            var dto = _mapper.Map<PlannedWeekMapActivity>(activity);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Add([FromBody] PlannedWeekMapActivityDTO activity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the associated Activity exists
            var existingActivity = _context.Activities.FirstOrDefault(a => a.ActivityID == activity.ActivityID);
            if (existingActivity == null)
                return NotFound(new { message = $"Activity with ID {activity.ActivityID} does not exist." });

            // Check if the associated PlannedWeekMap exists
            var existingMap = _context.PlannedWeekMaps.FirstOrDefault(p => p.PlannedWeekMapID == activity.PlannedWeekMapID);
            if (existingMap == null)
                return NotFound(new { message = $"PlannedWeekMap with ID {activity.PlannedWeekMapID} does not exist." });

            if (existingMap.DayStartTime > activity.StartTime || existingMap.DayEndTime < activity.EndTime)
                return BadRequest(new { message = "Activity start or end time is outside the PlannedWeekMap time range." });

            var entity = _mapper.Map<PlannedWeekMapActivity>(activity);
            entity.ActivityID = activity.ActivityID;
            entity.PlannedWeekMapID = activity.PlannedWeekMapID;
            _context.PlannedWeekMapActivities.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "PlannedWeekMapActivity added successfully!" });
        }

        [HttpPut("{plannedWeekMapActivityID}")]
        public IActionResult Edit(long plannedWeekMapActivityID, [FromBody] PlannedWeekMapActivityDTO updatedActivity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.PlannedWeekMapActivities.FirstOrDefault(pwma => pwma.PlannedWeekMapActivityID == plannedWeekMapActivityID);

            if (activity == null)
                return NotFound();

            _mapper.Map(updatedActivity, activity);

            _context.SaveChanges();

            return Ok(new { message = "PlannedWeekMapActivity updated successfully!" });
        }

        [HttpDelete("{plannedWeekMapActivityID}")]
        public IActionResult Delete(long plannedWeekMapActivityID)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.PlannedWeekMapActivities.FirstOrDefault(pwma => pwma.PlannedWeekMapActivityID == plannedWeekMapActivityID);

            if (activity == null)
                return NotFound();

            _context.PlannedWeekMapActivities.Remove(activity);
            _context.SaveChanges();

            return Ok(new { message = "PlannedWeekMapActivity deleted successfully!" });
        }
    }
}
