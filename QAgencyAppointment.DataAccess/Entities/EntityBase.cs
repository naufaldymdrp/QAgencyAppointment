using System.ComponentModel.DataAnnotations;

namespace QAgencyAppointment.DataAccess.Entities;

public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }
    
    public DateTime CreatedDate { get; set; }

    public DateTime ModifiedDate { get; set; }
}
