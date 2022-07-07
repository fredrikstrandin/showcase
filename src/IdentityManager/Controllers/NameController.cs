using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NameController : ControllerBase
    {
        public NameController()
        {
        }

        [HttpGet]
        public async  Task<ActionResult> Index()
        {
            return Ok("Kalle");
        }
    }
}
