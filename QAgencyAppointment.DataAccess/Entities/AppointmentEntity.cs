using System.ComponentModel.DataAnnotations;

namespace QAgencyAppointment.DataAccess.Entities;

public class AppointmentEntity : EntityBase
{
    public string Name { get; set; }
    
    public string TransactionId { get; set; }
    
    public ICollection<AppointmentRoomEntity> AppointmentRooms { get; set; }
}
