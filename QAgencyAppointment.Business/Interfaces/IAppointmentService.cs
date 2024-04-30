using QAgencyAppointment.Business.Dtos;
using QAgencyAppointment.DataAccess;

namespace QAgencyAppointment.Business.Interface;

public interface IAppointmentService
{
    Task<BusinessResult<string?>> CreateAppointment(AppointmentDtoRequest dtoResponse, string[] userIds);

    Task<List<AppointmentDtoResponse>> GetAllAppointments();

    List<AppointmentDtoResponse> GetAppointmentsByStartTime(DateTime startInterval, DateTime endInterval);
    
    List<AppointmentDtoResponse> GetAppointmentsByEndTime(DateTime startInterval, DateTime endInterval);

    List<AppointmentDtoResponse> GetAppointmentsByUserId(string userId);

    AppointmentDtoResponse GetAppointmentById(int appointmentId);

    AppointmentDtoResponse GetAppointmentByToken(string token);
}
