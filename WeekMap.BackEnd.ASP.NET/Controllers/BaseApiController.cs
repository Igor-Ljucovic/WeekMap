using Microsoft.AspNetCore.Mvc;

namespace WeekMap.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected bool TryGetUserId(out long userId)
        {
            userId = 0;
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (long.TryParse(claim, out userId)) return true;
            return long.TryParse(HttpContext.Session.GetString("UserID"), out userId);
        }
    }
}