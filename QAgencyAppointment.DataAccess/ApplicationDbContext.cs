using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using QAgencyAppointment.DataAccess.Entities;

namespace QAgencyAppointment.DataAccess;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configures relationship between AppointmentUserEntity to IdentityUser since IdentityUser does not have
        // navigation property to AppointmentUserEntity
        builder.Entity<AppointmentUserEntity>()
            .HasOne(s => s.IdentityUser)
            .WithMany()
            .HasForeignKey(e => e.UserEntityId)
            .IsRequired();
        
        base.OnModelCreating(builder);
    }

    public DbSet<AppointmentEntity> Appointments { get; set; }
    
    public DbSet<RoomEntity> Rooms { get; set; }
    
    public DbSet<AppointmentUserEntity> AppointmentUsers { get; set; }
    
    public DbSet<AppointmentRoomEntity> AppointmentRooms { get; set; }
}
