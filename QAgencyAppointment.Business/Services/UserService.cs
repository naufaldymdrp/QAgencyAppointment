using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QAgencyAppointment.Business.Dtos;
using QAgencyAppointment.Business.Interface;
using QAgencyAppointment.DataAccess.Interfaces;

namespace QAgencyAppointment.Business.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository; 
    private readonly TokenService _tokenService;

    public UserService(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<string?> Authenticate(LoginDto user)
    {
        if (user.Username.IsNullOrEmpty() || user.Password.IsNullOrEmpty())
        {
            return null;
        }
        
        IdentityUser? userEntity = await _userRepository.FindByUsernameAsync(user.Username);
        if (userEntity is null)
        {
            return null;
        }
        
        bool correctPassword = await _userRepository.CheckPasswordAsync(userEntity, user.Password);
        if (!correctPassword)
        {
            return null;
        }
        
        IList<string> roleNames = await _userRepository.GetAllRoleNamesByUserAsync(userEntity);
        
        // create claims
        List<Claim> claims = new() { new Claim(ClaimTypes.Name, user.Username) };
        claims.AddRange( roleNames.Select(rn => new Claim(ClaimTypes.Role, rn)));
        claims.Add(new Claim(ClaimTypes.Email, userEntity.Email ?? string.Empty));
        
        string jwt = _tokenService.GenerateJwtToken(claims);
        return jwt;
    }

}
