using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSettingsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserSettingsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            if (!HttpContext.Session.TryGetValue("UserID", out _))
                return Unauthorized(new { message = "User not logged in." });

            var settings = _context.UserSettings.FirstOrDefault(us => us.UserID == id);

            if (settings == null)
                return NotFound();

            return Ok(settings);
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] UserSettingsDTO updated)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest();

            var settings = _context.UserSettings.FirstOrDefault(u => u.UserID == id);

            if (settings == null) 
                return NotFound();

            _mapper.Map(updated, settings);

            _context.SaveChanges();

            return Ok(new { message = "UserSettings updated successfully!" });
        }
    }
}