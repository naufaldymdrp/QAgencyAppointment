using Microsoft.AspNetCore.Identity;

namespace QAgencyAppointment.DataAccess.Interfaces;

public interface IUserRepository
{
    public Task<IdentityUser> FindByUsernameAsync(string username);

    public Task<bool> CheckPasswordAsync(IdentityUser user, string password);

    public Task<IList<string>> GetAllRoleNamesByUserAsync(IdentityUser user);
}
