using QAgencyAppointment.DataAccess.Entities;

namespace QAgencyAppointment.DataAccess.Interfaces;

public interface IAppointmentRepository
{
    Task<RepositoryResult> CreateAppointment(AppointmentEntity entity);
    
    List<AppointmentEntity> GetAllAppointments();
    
    List<AppointmentEntity> GetAllUpcomingAppointments();

    List<AppointmentEntity> GetAppointmentsByStartMeetingTime(DateTime startInterval, DateTime endInterval);
    
    List<AppointmentEntity> GetAppointmentsByEndMeetingTime(DateTime startInterval, DateTime endInterval);

    AppointmentEntity GetAppointmentById(int id);

    AppointmentEntity GetAppointmentByToken(string token);
}
