using AutoMapper;
using WeekMap.Data;
using WeekMap.Models;
using Microsoft.AspNetCore.Mvc;
using WeekMap.DTOs;

namespace WeekMap.Controllers
{
    namespace Weekmap.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class RealisedWeekMapController : ControllerBase
        {
            private readonly AppDbContext _context;
            private readonly IMapper _mapper;

            public RealisedWeekMapController(AppDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                var maps = _context.RealisedWeekMaps.ToList();
                return Ok(maps);
            }

            [HttpPost]
            public IActionResult Add([FromBody] RealisedWeekMapDTO map)
            {
                if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                    return Unauthorized(new { message = "User not logged in." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var entity = _mapper.Map<RealisedWeekMap>(map);
                entity.PlannedWeekMapID = map.PlannedWeekMapID;
                _context.RealisedWeekMaps.Add(entity);

                _context.SaveChanges();

                return Ok(new { message = "RealisedWeekMap added successfully!" });
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(long id)
            {

                if (!long.TryParse(HttpContext.Session.GetString("UserID"), out long userId))
                    return Unauthorized(new { message = "User not logged in." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var map = _context.RealisedWeekMaps.FirstOrDefault(r => r.RealisedWeekMapID == id);
                if (map == null)
                    return NotFound();

                _context.RealisedWeekMaps.Remove(map);
                _context.SaveChanges();

                return Ok(new { message = "RealisedWeekMap deleted successfully!" });
            }
        }
    }
}
