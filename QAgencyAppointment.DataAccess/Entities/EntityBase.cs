using System.ComponentModel.DataAnnotations;

namespace QAgencyAppointment.DataAccess.Entities;

public abstract class EntityBase
{
    [Key]
    public int Id { get; set; }

    public bool IsDeleted { get; set; } = false;
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? ModifiedDate { get; set; }
}
