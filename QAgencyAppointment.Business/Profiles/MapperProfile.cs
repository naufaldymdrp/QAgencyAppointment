using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QAgencyAppointment.Business.Dtos;
using QAgencyAppointment.DataAccess.Entities;

namespace QAgencyAppointment.Business.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserDto, IdentityUser>();
        CreateMap<IdentityUser, UserDto>();

        CreateMap<AppointmentDtoResponse, AppointmentEntity>();
        CreateMap<AppointmentEntity, AppointmentDtoResponse>();

        CreateMap<AppointmentDtoRequest, AppointmentDtoResponse>();
        CreateMap<AppointmentDtoResponse, AppointmentDtoRequest>();
    }
}