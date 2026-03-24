using Microsoft.AspNetCore.Mvc;

namespace WeekMap.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected bool TryGetUserId(out long userId)
        {
            userId = 0;
            return long.TryParse(HttpContext.Session.GetString("UserID"), out userId);
        }
    }
}