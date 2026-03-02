using Common.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Controllers
{
    public class BaseController
    {
        [Route("api/v{version:apiVersion}/[controller]")]
        [Authorize]
        [ApiController]
        [ValidationModelAttribute]
        public abstract class ApiController : Controller
        {
            protected ClaimsIdentity? Identity => HttpContext.User.Identity as ClaimsIdentity;
        }
    }
}
