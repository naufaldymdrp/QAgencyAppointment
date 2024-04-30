using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using QAgencyAppointment.DataAccess.Entities;
using QAgencyAppointment.DataAccess.Interfaces;

namespace QAgencyAppointment.DataAccess.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AppointmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RepositoryResult> CreateAppointment(AppointmentEntity entity)
    {
        try
        {
            _dbContext.Appointments.Add(entity);
            await _dbContext.SaveChangesAsync();

            return new RepositoryResult(true);
        }
        catch (Exception e)
        {
            return new RepositoryResult(false, "Error during creation of appointment data");
        }
    }

    public List<AppointmentEntity> GetAllAppointments()
    {
        List<AppointmentEntity> appointmentEntities = _dbContext.Appointments
            .Include(x => x.AppointmentUsers)
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .ToList();

        return appointmentEntities;
    }

    public List<AppointmentEntity> GetAllUpcomingAppointments()
        => _dbContext.Appointments
            .Include(x => x.AppointmentUsers)
            .Where(x => !x.IsDeleted && x.StarTime <= DateTime.Now.AddHours(-2))
            .OrderByDescending(x => x.Id)
            .ToList();
    

    public List<AppointmentEntity> GetAppointmentsByStartMeetingTime(DateTime startInterval, DateTime endInterval)
        => _dbContext.Appointments
            .Where(x => !x.IsDeleted
                        && x.StarTime >= startInterval && x.StarTime <= endInterval)
            .ToList();

    public List<AppointmentEntity> GetAppointmentsByEndMeetingTime(DateTime startInterval, DateTime endInterval)
        => _dbContext.Appointments
            .Where(x => !x.IsDeleted
                        && x.EndTime >= startInterval && x.EndTime <= endInterval)
            .ToList();

    public AppointmentEntity? GetAppointmentById(int id)
        => _dbContext.Appointments.FirstOrDefault(x => !x.IsDeleted && x.Id == id);

    public AppointmentEntity GetAppointmentByToken(string token)
        => _dbContext.Appointments.FirstOrDefault(x => !x.IsDeleted && x.Token == token);
}

