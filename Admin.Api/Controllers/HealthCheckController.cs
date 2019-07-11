using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        // GET api/healthcheck
        [HttpGet]
        public ActionResult<string> Get()
        {
            const string message = "PartnerUser Api Service is healthy!";

            return message;
        }
    }
}