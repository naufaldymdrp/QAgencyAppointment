using QAgencyAppointment.DataAccess.Entities;

namespace QAgencyAppointment.DataAccess.Interfaces;

public interface IAppointmentUserRepository
{
    Task<RepositoryResult> LinkAppointmentToUser(int appointmentId, string[] userId);
    
    List<AppointmentUserEntity> GetAllAppointmentUsers();

    List<AppointmentUserEntity> GetAppointmentUsersByAppointmentId(int appointmentId);

    List<AppointmentUserEntity> GetAppointmentUsersByUserId(string userId);
}