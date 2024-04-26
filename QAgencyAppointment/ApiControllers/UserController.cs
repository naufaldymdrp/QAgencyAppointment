using Microsoft.AspNetCore.Mvc;
using QAgencyAppointment.Business.Dtos;

namespace QAgencyAppointment.ApiControllers; 

[ApiController]
[Route("/api/[controller]s")]
public class UserController : ControllerBase
{
    [HttpGet("get")]
    public IActionResult GetUserData()
    {
        return Ok("Okeh");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto userDto)
    {
        return Ok(userDto);
    }
}
