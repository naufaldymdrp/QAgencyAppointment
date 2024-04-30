using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QAgencyAppointment.DataAccess.Entities;

public class AppointmentRoomEntity : EntityBase
{
    [Column("AppointmentId")]
    public int AppointmentEntityId { get; set; }
    
    [Column("RoomId")]
    public int RoomEntityId { get; set; }
    
    public AppointmentEntity AppointmentEntity { get; set; }
    
    public RoomEntity RoomEntity { get; set; }
}