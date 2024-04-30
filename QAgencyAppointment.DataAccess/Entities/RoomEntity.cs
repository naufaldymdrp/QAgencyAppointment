using System.ComponentModel.DataAnnotations;

namespace QAgencyAppointment.DataAccess.Entities;

public class RoomEntity : EntityBase
{
    public string Name { get; set; }
    
    public ICollection<AppointmentRoomEntity> AppointmentRooms { get; set; }
}