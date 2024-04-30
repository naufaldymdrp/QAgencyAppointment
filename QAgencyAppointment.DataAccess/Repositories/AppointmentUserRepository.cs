using Microsoft.EntityFrameworkCore;
using QAgencyAppointment.DataAccess.Entities;
using QAgencyAppointment.DataAccess.Interfaces;

namespace QAgencyAppointment.DataAccess.Repositories;

public class AppointmentUserRepository : IAppointmentUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AppointmentUserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RepositoryResult> LinkAppointmentToUser(int appointmentId, string[] userIds)
    {
        try
        {
            List<AppointmentUserEntity> entities = new();
            foreach (string userId in userIds)
            {
                entities.Add(new AppointmentUserEntity()
                {
                    AppointmentEntityId = appointmentId,
                    UserEntityId = userId,
                    IsDeleted = false,
                    CreatedDate = DateTime.Now
                });
            }
            
            await _dbContext.AppointmentUsers.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();

            return new RepositoryResult(true);
        }
        catch (Exception e)
        {
            return new RepositoryResult(false, "Error during linking of Appointment to User.", e.Message);
        }
    }

    public List<AppointmentUserEntity> GetAllAppointmentUsers()
        => _dbContext.AppointmentUsers.Where(x => !x.IsDeleted).ToList();

    public List<AppointmentUserEntity> GetAppointmentUsersByAppointmentId(int appointmentId)
        => _dbContext.AppointmentUsers
            .Where(x => !x.IsDeleted && x.AppointmentEntityId == appointmentId)
            .ToList();

    public List<AppointmentUserEntity> GetAppointmentUsersByUserId(string userId)
        => _dbContext.AppointmentUsers
            .Include(x => x.AppointmentEntity)
            .Include(x => x.UserEntityId)
            .Where(x => !x.IsDeleted && x.UserEntityId == userId)
            .ToList();
}