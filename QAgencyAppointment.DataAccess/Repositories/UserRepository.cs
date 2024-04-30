using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QAgencyAppointment.DataAccess.Entities;
using QAgencyAppointment.DataAccess.Interfaces;

namespace QAgencyAppointment.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public async Task<IdentityUser> FindByUserIdAsync(string userId)
    {
        IdentityUser userEntity = await _userManager.FindByIdAsync(userId);
        return userEntity;
    }

    public async Task<IdentityUser?> FindByUsernameAsync(string username)
        => await _userManager.FindByNameAsync(username);

    public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        => await _userManager.CheckPasswordAsync(user, password);

    public async Task<IList<string>> GetAllRoleNamesByUserAsync(IdentityUser user)
        => await _userManager.GetRolesAsync(user);

    public async Task<IList<string>> GetRoleNamesByUserAsync(IdentityUser userEntity)
        => await _userManager.GetRolesAsync(userEntity);

    public async Task<IList<string>> GetRoleNamesByUserIdAsync(string userId)
    {
        List<IdentityUserRole<string>> userRoles = _dbContext.UserRoles
            .Where(u => u.UserId == userId)
            .ToList();

        List<string> roleNames = _dbContext.Roles
            .Where(r => userRoles.Select(ur => ur.RoleId).Contains(r.Id))
            .Select(r => r.Name)
            .ToList();
        return roleNames;
    }
}
