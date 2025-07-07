using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WeekMap.Data;
using WeekMap.DTOs;
using WeekMap.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WeekMap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlannedWeekMapController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PlannedWeekMapController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var maps = _context.PlannedWeekMaps.Where(p => p.UserID == userId).ToList();
            return Ok(maps);
        }

        [HttpPost]
        public IActionResult Add([FromBody] PlannedWeekMapDTO map)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            map.UserID = userId;

            var entity = _mapper.Map<PlannedWeekMap>(map);
            entity.UserID = userId;
            _context.PlannedWeekMaps.Add(entity);
            _context.SaveChanges();

            return Ok(new { message = "PlannedWeekMap added successfully!" });
        }

        [HttpPut("{id}")]
        public IActionResult Edit(long id, [FromBody] PlannedWeekMapDTO updatedMap)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var map = _context.PlannedWeekMaps.FirstOrDefault(p => p.PlannedWeekMapID == id);
            if (map == null)
                return NotFound();

            _mapper.Map(updatedMap, map);

            _context.SaveChanges();
            return Ok(new { message = "PlannedWeekMap updated successfully!" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                return Unauthorized(new { message = "User not logged in." });

            var map = _context.PlannedWeekMaps.FirstOrDefault(p => p.PlannedWeekMapID == id);
            if (map == null)
                return NotFound();

            _context.PlannedWeekMaps.Remove(map);
            _context.SaveChanges();
            return Ok(new { message = "PlannedWeekMap deleted successfully!" });
        }
    }
}