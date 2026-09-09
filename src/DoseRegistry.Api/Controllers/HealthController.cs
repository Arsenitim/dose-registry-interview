using Microsoft.AspNetCore.Mvc;

namespace DoseRegistry.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> Get() => Ok(new { status = "ok" });
}
