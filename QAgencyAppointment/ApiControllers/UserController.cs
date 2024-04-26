using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet("/get")]
    public IActionResult GetUserData()
    {
        return Ok("Ok");
    }
}
