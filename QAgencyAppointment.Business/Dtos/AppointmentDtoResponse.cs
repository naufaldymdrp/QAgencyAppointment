namespace QAgencyAppointment.Business.Dtos;

public class AppointmentDtoRequest
{
    public string Name { get; set; }

    public bool RoomAppointment { get; set; }
    
    public DateTime StarTime { get; set; }
    
    public DateTime EndTime { get; set; }
}

public class AppointmentDtoResponse : AppointmentDtoRequest
{
    public string? Token { get; set; }
    public List<UserDto>? Users { get; set; }
}