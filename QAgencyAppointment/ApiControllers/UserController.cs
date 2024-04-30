using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QAgencyAppointment.Business.Dtos;
using QAgencyAppointment.Business.Interface;

namespace QAgencyAppointment.ApiControllers; 

[Authorize]
[ApiController]
[Route("/api/[controller]s")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("get")]
    public IActionResult GetUserData()
    {
        return Ok("Okeh");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto userDto)
    {
        string? token = await _userService.Authenticate(userDto);
        if (token is null) return Unauthorized();

        return Ok(new
        {
            Token = token
        });
    }
}
