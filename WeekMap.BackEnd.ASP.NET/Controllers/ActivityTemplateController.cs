using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityTemplateController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ActivityTemplateController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activities = _context.ActivityTemplates.Where(a => a.UserID == userId).ToList();

            return Ok(activities);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ActivityTemplateDTO activity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            activity.UserID = userId;

            var entity = _mapper.Map<ActivityTemplate>(activity);

            entity.UserID = userId;
            _context.ActivityTemplates.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "Activity template added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] ActivityTemplateDTO updatedActivity)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.ActivityTemplates.FirstOrDefault(a => a.ActivityTemplateID == id);
            if (activity == null)
                return NotFound();

            var x = activity.UserID;

            _mapper.Map(updatedActivity, activity);
            activity.UserID = userId;

            _context.SaveChanges();

            return Ok(new { message = "Activity template updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var activity = _context.ActivityTemplates.FirstOrDefault(a => a.ActivityTemplateID == id);
            if (activity == null)
                return NotFound();

            _context.ActivityTemplates.Remove(activity);
            _context.SaveChanges();

            return Ok(new { message = "Activity template deleted successfully!" });
        }
    }
}

