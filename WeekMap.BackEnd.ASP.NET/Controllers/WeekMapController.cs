using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeekMap.Data;
using WeekMap.DTOs;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeekMapController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public WeekMapController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var maps = _context.WeekMaps.Where(p => p.UserID == userId).ToList();
            return Ok(maps);
        }

        [HttpGet("{id}")] 

        public IActionResult GetById(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long sessionUserID) || sessionUserID != id)
                return Unauthorized(new { message = "Unauthorized access." });

            var map = _context.WeekMaps
                .Where(p => p.UserID == id)
                .OrderByDescending(p => p.DateCreated)
                .FirstOrDefault();

            if (map == null)
                return NotFound(new { message = "No week maps found for user." });

            return Ok(map);
        }

        [HttpGet("{id}/activityTemplates")] // idk whether or not to put it in WeekMapActivity or here in PlannedWeekMap controller
        public IActionResult GetWeekMapActivities(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long sessionUserID))
                return Unauthorized(new { message = "User not logged in." });

            var activities = _context.WeekMapActivities
            .Include(pwma => pwma.ActivityTemplate)
                .ThenInclude(a => a.ActivityCategory)
            .Where(a => a.WeekMapID == id)
            .ToList();

            return Ok(activities);
        }

        [HttpPost]
        public IActionResult Add([FromBody] WeekMapDTO map)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            map.UserID = userId;

            var entity = _mapper.Map<Models.WeekMap>(map);
            entity.UserID = userId;
            _context.WeekMaps.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "Week map added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] WeekMapDTO updatedMap)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var map = _context.WeekMaps.FirstOrDefault(p => p.WeekMapID == id);
            if (map == null)
                return NotFound();

            _mapper.Map(updatedMap, map);

            _context.SaveChanges();
            return Ok(new { message = "Week map updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var map = _context.WeekMaps.FirstOrDefault(p => p.WeekMapID == id);
            if (map == null)
                return NotFound();

            _context.WeekMaps.Remove(map);
            _context.SaveChanges();
            return Ok(new { message = "Week map deleted successfully!" });
        }
    }
}