using QAgencyAppointment.Business.Dtos;

namespace QAgencyAppointment.Models;

public class AppointmentRequestModel
{
    public AppointmentDtoRequest AppointmentRequestData { get; set; }
    
    public string[] UserIds { get; set; }
}