using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QAgencyAppointment.DataAccess.Entities;
using QAgencyAppointment.DataAccess.Interfaces;

namespace QAgencyAppointment.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityUser?> FindByUsernameAsync(string username)
        => await _userManager.FindByNameAsync(username);

    public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        => await _userManager.CheckPasswordAsync(user, password);

    public async Task<IList<string>> GetAllRoleNamesByUserAsync(IdentityUser user)
        => await _userManager.GetRolesAsync(user);
}
