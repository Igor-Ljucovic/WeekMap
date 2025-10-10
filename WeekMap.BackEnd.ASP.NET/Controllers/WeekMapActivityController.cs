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
    public class WeekMapActivityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WeekMapActivityController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{weekMapActivityID}")]
        public IActionResult GetById(long weekMapActivityID)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.WeekMapActivities
                .FirstOrDefault(pwma => pwma.WeekMapActivityID == weekMapActivityID);

            if (activity == null)
                return NotFound();

            var dto = _mapper.Map<WeekMapActivity>(activity);
            return Ok(dto);
        }

        [HttpPost]
        public IActionResult Add([FromBody] WeekMapActivityDTO activity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if the associated ActivityTemplate exists
            var existingActivity = _context.ActivityTemplates.FirstOrDefault(a => a.ActivityTemplateID == activity.ActivityTemplateID);
            if (existingActivity == null)
                return NotFound(new { message = $"ActivityTemplate with ID {activity.ActivityTemplateID} does not exist." });

            // Check if the associated PlannedWeekMap exists
            var existingMap = _context.WeekMaps.FirstOrDefault(p => p.WeekMapID == activity.WeekMapID);
            if (existingMap == null)
                return NotFound(new { message = $"PlannedWeekMap with ID {activity.WeekMapID} does not exist." });

            if (existingMap.DayStartTime > activity.StartTime || existingMap.DayEndTime < activity.EndTime)
                return BadRequest(new { message = "Activity start or end time is outside the PlannedWeekMap time range." });

            var entity = _mapper.Map<WeekMapActivity>(activity);
            entity.ActivityTemplateID = activity.ActivityTemplateID;
            entity.WeekMapID = activity.WeekMapID;
            _context.WeekMapActivities.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "Activity added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] WeekMapActivityDTO updatedActivity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.WeekMapActivities.FirstOrDefault(pwma => pwma.WeekMapActivityID == id);

            if (activity == null)
                return NotFound();

            _mapper.Map(updatedActivity, activity);

            _context.SaveChanges();

            return Ok(new { message = "Activity updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.WeekMapActivities.FirstOrDefault(pwma => pwma.WeekMapActivityID == id);

            if (activity == null)
                return NotFound();

            _context.WeekMapActivities.Remove(activity);
            _context.SaveChanges();

            return Ok(new { message = "Activity deleted successfully!" });
        }
    }
}
