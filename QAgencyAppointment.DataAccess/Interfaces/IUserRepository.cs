using Microsoft.AspNetCore.Identity;

namespace QAgencyAppointment.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<IdentityUser> FindByUserIdAsync(string userId);
    
    Task<IdentityUser> FindByUsernameAsync(string username);

    Task<bool> CheckPasswordAsync(IdentityUser user, string password);

    Task<IList<string>> GetAllRoleNamesByUserAsync(IdentityUser user);
}
