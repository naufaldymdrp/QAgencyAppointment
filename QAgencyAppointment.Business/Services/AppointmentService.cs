using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QAgencyAppointment.Business.Dtos;
using QAgencyAppointment.Business.Interface;
using QAgencyAppointment.DataAccess;
using QAgencyAppointment.DataAccess.Entities;
using QAgencyAppointment.DataAccess.Interfaces;

namespace QAgencyAppointment.Business.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAppointmentUserRepository _appointmentUserRepository;

    public AppointmentService(IMapper mapper, IUserRepository userRepository, IAppointmentRepository appointmentRepository,
        IAppointmentUserRepository appointmentUserRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _appointmentRepository = appointmentRepository;
        _appointmentUserRepository = appointmentUserRepository;
    }

    public async Task<BusinessResult<string?>> CreateAppointment(AppointmentDtoRequest dtoRequest, string[] userIds)
    {
        // Generate appointment token
        AppointmentDtoResponse dtoResponse = _mapper.Map<AppointmentDtoResponse>(dtoRequest);
        dtoResponse.Token = Guid.NewGuid().ToString();
        
        AppointmentEntity entity = _mapper.Map<AppointmentEntity>(dtoResponse);

        bool correctTimeInterval = await ValidateTimeIntervalAsync(dtoRequest.StarTime, dtoRequest.EndTime);
        if (!correctTimeInterval)
        {
            return new BusinessResult<string?>(false, null, "Time interval is not correct");
        }

        RepositoryResult result = await _appointmentRepository.CreateAppointment(entity);
        if (!result.Success)
        {
            return new BusinessResult<string?>(false, null, result.ErrorMessage);
        }
        
        RepositoryResult result2 = await _appointmentUserRepository.LinkAppointmentToUser(entity.Id, userIds);
        if (!result2.Success)
        {
            return new BusinessResult<string?>(false, null, result.ErrorMessage);
        }

        return new BusinessResult<string?>(true, dtoResponse.Token, null);
    }

    public async Task<bool> ValidateTimeIntervalAsync(DateTime startTime, DateTime endTime)
    {
        if (startTime.Day != endTime.Day)
        {
            return false;
        }

        TimeSpan appointmentDuration = endTime - startTime;

        // Check for overlapping time intervals
        List<AppointmentDtoResponse> upcomingAppointmentDtoList = await GetAllAppointments(upcomingAppointment: true);
        if (upcomingAppointmentDtoList.Count > 0)
        {
            List<(TimeSpan, TimeSpan)> existingIntervals = upcomingAppointmentDtoList
                .Select(a => (a.StarTime.TimeOfDay, a.EndTime.TimeOfDay))
                .ToList();
            bool timeOverlap = existingIntervals.Any(e => startTime.TimeOfDay < e.Item2 && e.Item1 < endTime.TimeOfDay);
            if (timeOverlap)
            {
                return false;
            }
        }

        return true;
    }
    
    public async Task<List<AppointmentDtoResponse>> GetAllAppointments(bool upcomingAppointment = false)
    {
        List<AppointmentEntity> appointmentEntities = upcomingAppointment ? 
            _appointmentRepository.GetAllUpcomingAppointments() : _appointmentRepository.GetAllAppointments();
        List<AppointmentDtoResponse> dtoList = new();
        
        // Now we need to assign each appointment users
        for (int i = 0; i < appointmentEntities.Count(); i++)
        {
            var appointmentDtoResponse = await LinkAppointmentsToUsersAsync(appointmentEntities[i]);
            
            dtoList.Add(appointmentDtoResponse);
        }
        
        return dtoList;
    }

    public async Task<AppointmentDtoResponse> LinkAppointmentsToUsersAsync(AppointmentEntity currentAppointmentEntity)
    {
        int userCount = currentAppointmentEntity.AppointmentUsers.Count();
        List<IdentityUser> userEntities = new();
        for (int j = 0; j < userCount; j++)
        {
            var currentUser = currentAppointmentEntity.AppointmentUsers[j];
            var userEntity = await _userRepository.FindByUserIdAsync(currentUser.UserEntityId);
            userEntities.Add(userEntity);
        }

        AppointmentDtoResponse appointmentDtoResponse = _mapper.Map<AppointmentDtoResponse>(currentAppointmentEntity);
        appointmentDtoResponse.Users = userEntities.Select(x => _mapper.Map<UserDto>(x)).ToList();
        return appointmentDtoResponse;
    }

    public List<AppointmentDtoResponse> GetAppointmentsByStartTime(DateTime startInterval, DateTime endInterval)
    {
        var entityList = _appointmentRepository.GetAppointmentsByStartMeetingTime(startInterval, endInterval);
        var dtoList = entityList.Select(x =>  _mapper.Map<AppointmentDtoResponse>(x))
            .ToList();
        
        return dtoList;
    }

    public List<AppointmentDtoResponse> GetAppointmentsByEndTime(DateTime startInterval, DateTime endInterval)
    {
        var entityList = _appointmentRepository.GetAppointmentsByStartMeetingTime(startInterval, endInterval);
        var dtoList = entityList.Select(x =>  _mapper.Map<AppointmentDtoResponse>(x))
            .ToList();
        
        return dtoList;
    }

    public List<AppointmentDtoResponse> GetAppointmentsByUserId(string userId)
    {
        List<AppointmentUserEntity> entityList = _appointmentUserRepository.GetAppointmentUsersByUserId(userId);
        List<AppointmentDtoResponse> dtoList = entityList
            .Select(x => _mapper.Map<AppointmentDtoResponse>(x.AppointmentEntity))
            .ToList();

        return dtoList;
    }

    public AppointmentDtoResponse GetAppointmentById(int appointmentId)
    {
        AppointmentEntity entity = _appointmentRepository.GetAppointmentById(appointmentId);
        AppointmentDtoResponse dtoResponse = _mapper.Map<AppointmentDtoResponse>(entity);
        
        return dtoResponse;
    }

    public AppointmentDtoResponse GetAppointmentByToken(string token)
    {
        AppointmentEntity entity = _appointmentRepository.GetAppointmentByToken(token);
        AppointmentDtoResponse dtoResponse = _mapper.Map<AppointmentDtoResponse>(entity);
        
        return dtoResponse;
    }
}
