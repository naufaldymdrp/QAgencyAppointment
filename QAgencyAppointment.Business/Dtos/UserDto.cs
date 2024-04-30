using Microsoft.AspNetCore.Identity;

namespace QAgencyAppointment.Business.Dtos;

public class UserDto
{
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public List<IdentityRole>? Roles { get; set; }
}