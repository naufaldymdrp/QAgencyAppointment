using System.ComponentModel.DataAnnotations;

namespace QAgencyAppointment.DataAccess.Entities;

public class AppointmentEntity : EntityBase
{
    public string Name { get; set; }
    
    public string Token { get; set; }
    
    public bool RoomAppointment { get; set; }
    
    public DateTime StarTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public List<AppointmentRoomEntity>? AppointmentRooms { get; set; }
    
    public List<AppointmentUserEntity>? AppointmentUsers { get; set; }
}
