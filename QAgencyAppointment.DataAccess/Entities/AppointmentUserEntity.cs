using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace QAgencyAppointment.DataAccess.Entities;

public class AppointmentUserEntity : EntityBase
{
    [Column("AppointmentId")]
    public int AppointmentEntityId { get; set; }
    
    [Column("UserId")]
    public string UserEntityId { get; set; }
    
    public AppointmentEntity AppointmentEntity { get; set; }
    
    public IdentityUser IdentityUser { get; set; }
}