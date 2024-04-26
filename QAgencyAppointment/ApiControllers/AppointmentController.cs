using Microsoft.AspNetCore.Mvc;

namespace QAgencyAppointment.ApiControllers;

[ApiController]
[Route("api/[controller]s")]
public class AppointmentController : ControllerBase
{
    [HttpGet("get")]
    public IActionResult Get()
    {
        return Ok("Ok");
    }
}
