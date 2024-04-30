using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QAgencyAppointment.Business.Dtos;
using QAgencyAppointment.Business.Interface;
using QAgencyAppointment.Models;

namespace QAgencyAppointment.ApiControllers;

[Authorize]
[ApiController]
[Route("api/[controller]s")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }
    
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        List<AppointmentDtoResponse> dtoList = await _appointmentService.GetAllAppointments();
        
        return Ok(dtoList);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequestModel requestModel)
    {
        var result = await _appointmentService.CreateAppointment(requestModel.AppointmentRequestData, requestModel.UserIds);
        if (!result.Success)
        {
            return Problem(result.BusinessError);
        }

        return Ok(result);
    }
}
