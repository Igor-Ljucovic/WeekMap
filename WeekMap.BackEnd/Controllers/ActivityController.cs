using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activities = _context.Activities.Where(a => a.UserID == userId).ToList();

            return Ok(activities);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ActivityDTO activity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            activity.UserID = userId;

            var entity = _mapper.Map<Activity>(activity);

            entity.UserID = userId; // Ensure the UserID is set to the current user
            _context.Activities.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "Activity added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] ActivityDTO updatedActivity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.Activities.FirstOrDefault(a => a.ActivityID == id);
            if (activity == null)
                return NotFound();

            var x = activity.UserID;

            _mapper.Map(updatedActivity, activity);
            activity.UserID = userId; // Ensure the UserID is set to the current user

            _context.SaveChanges();

            return Ok(new { message = "Activity updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.Activities.FirstOrDefault(a => a.ActivityID == id);
            if (activity == null)
                return NotFound();

            _context.Activities.Remove(activity);
            _context.SaveChanges();

            return Ok(new { message = "Activity deleted successfully!" });
        }
    }
}

