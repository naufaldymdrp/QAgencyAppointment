using Microsoft.AspNetCore.Identity;
using QAgencyAppointment.DataAccess.Interfaces;

namespace QAgencyAppointment.DataAccess.Seedings;

public class DataSeeding : ISeeding
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public DataSeeding(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    private const string AdminRoleId = "4cecdd4a-00b3-45c0-b3f4-d52b32cb4350";
    private const string EmployeeRoleId = "7d44b1eb-083c-4e37-8f6d-6386f6bd8035";
    private const string CustomerRoleId = "0786d086-ba17-43c9-948b-d22533245a2c";

    private const string AdminUserId = "b2af0e50-2b99-4868-93e2-4d641ceb8fa2";
    private const string Employee1UserId = "498ce69b-fb8e-4257-b257-77a30eef35ef";
    private const string Customer1UserId = "71f14191-d4d8-44da-87d2-38658dbe514a";

    public static List<IdentityRole> RoleSeeds
    {
        get
        {
            List<IdentityRole> roleSeeds = new()
            {
                new IdentityRole()
                {
                    Id = AdminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole()
                {
                    Id = EmployeeRoleId,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole()
                {
                    Id = CustomerRoleId,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
            };

            return roleSeeds;
        }
    }

    public bool SeedRoles()
    {
        List<IdentityRole> roleSeeds = RoleSeeds;
        
        // Remove role seeds that already exist in the database
        int deletedSeeds = roleSeeds.RemoveAll( 
            rs => _dbContext.Roles.Any(r => r.Name.Equals(rs.Name)));

        if (deletedSeeds < roleSeeds.Count())
        {
            _dbContext.Roles.AddRange(roleSeeds);
            _dbContext.SaveChanges(); // blocking
            return true;
        }

        return false;
    }

    public static List<IdentityUser> UserSeeds
    {
        get
        {
            List<IdentityUser> userSeeds = new()
            {
                new IdentityUser()
                {
                    Id = AdminUserId,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },
                new IdentityUser()
                {
                    Id = Employee1UserId,
                    UserName = "employee1",
                    NormalizedUserName = "EMPLOYEE1",
                    Email = "employee1@example.com",
                    NormalizedEmail = "EMPLOYEE1@EXAMPLE.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },
                new IdentityUser()
                {
                    Id = Customer1UserId,
                    UserName = "customer1",
                    NormalizedUserName = "CUSTOMER1",
                    Email = "customer1@example.com",
                    NormalizedEmail = "CUSTOMER1@EXAMPLE.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0
                },
            };

            return userSeeds;
        }
    }

    public bool SeedUsers()
    {
        List<IdentityUser> userSeeds = UserSeeds;
        
        // Ordering matters, make sure that this ordering matches userSeeds ordering
        List<string> rawPasswords = new()
        {
            "admin#12345",
            "Employee1#12345",
            "Customer1#12345"
        };

        // Creates password
        for (int i = 0; i < userSeeds.Count(); i++)
        {
            userSeeds[i].PasswordHash = _userManager.PasswordHasher.HashPassword(userSeeds[i], rawPasswords[i]);
        }

        // Remove user seeds that already exist in the database
        int deletedSeeds = userSeeds.RemoveAll(
            us => _dbContext.Users.Any(u => u.UserName.Equals(us.UserName)));

        // Only seed if user seeds still exist
        if (deletedSeeds < userSeeds.Count())
        {
            _dbContext.Users.AddRange(userSeeds);
            _dbContext.SaveChanges(); // blocking
            return true;
        }

        return false;
    }

    public static List<IdentityUserRole<string>> UserRoleSeeds
    {
        get
        {
            List<IdentityUserRole<string>> userRoleSeeds = new()
            {
                new()
                {
                    RoleId = AdminRoleId,
                    UserId = AdminUserId,
                },
                new()
                {
                    RoleId = EmployeeRoleId,
                    UserId = Employee1UserId
                },
                new()
                {
                    RoleId = CustomerRoleId,
                    UserId = Customer1UserId
                }
            };

            return userRoleSeeds;
        }
    }

    public bool SeedUserRoles()
    {
        List<IdentityUserRole<string>> userRoleSeeds = UserRoleSeeds;

        int deletedSeeds = userRoleSeeds.RemoveAll(
            urs => _dbContext.UserRoles
                    .Any(ur => ur.RoleId == urs.RoleId && ur.UserId == urs.UserId));

        if (deletedSeeds < userRoleSeeds.Count())
        {
            _dbContext.UserRoles.AddRange(userRoleSeeds);
            _dbContext.SaveChanges(); // blocking
            return true;
        }

        return false;
    }
}