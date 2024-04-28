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

    // protected override void OnModelCreating(ModelBuilder builder)
    // {
    // }

    // public DbSet<UserEntity> Users { get; set; }

    // public DbSet<AppointmentEntity> Appointments { get; set; }
}
