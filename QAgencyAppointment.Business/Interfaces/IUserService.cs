using QAgencyAppointment.Business.Dtos;

namespace QAgencyAppointment.Business.Interface;

public interface IUserService
{
    Task<string?> Authenticate(LoginDto user);
}

