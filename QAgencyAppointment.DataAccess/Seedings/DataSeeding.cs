using Microsoft.AspNetCore.Identity;
using QAgencyAppointment.DataAccess.Entities;
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
    private const string Customer2UserId = "82g25202-e3e6-44zf-8712-49748dbe625b";

    private const int SampleAppointment1Id = 1; // employee 1 with customer 1
    private const int SampleAppointment2Id = 2; // employee 1 with customer 2

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
                new IdentityUser()
                {
                    Id = Customer2UserId,
                    UserName = "customer2",
                    NormalizedUserName = "CUSTOMER2",
                    Email = "customer2@example.com",
                    NormalizedEmail = "CUSTOMER2@EXAMPLE.COM",
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
        int seedsCount = userSeeds.Count();
        
        // Ordering matters, make sure that this ordering matches userSeeds ordering
        List<string> rawPasswords = new()
        {
            "admin#12345",
            "Employee1#12345",
            "Customer1#12345",
            "Customer2#98765"
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
        if (deletedSeeds < seedsCount)
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

    public bool SeedAppointments()
    {
        List<AppointmentEntity> appointmentEntities = new()
        {
            new AppointmentEntity()
            {
                Id = SampleAppointment1Id,
                Name = "Sample Appointment 1",
                Token = Guid.NewGuid().ToString(),
                StarTime = DateTime.Parse("29/04/2024 08:00:00"),
                EndTime = DateTime.Parse("29/04/2024 09:00:00"),
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            },
            new AppointmentEntity()
            {
                Id = SampleAppointment2Id,
                Name = "Sample Appointment 2",
                Token = Guid.NewGuid().ToString(),
                StarTime = DateTime.Parse("29/04/2024 09:00:00"),
                EndTime = DateTime.Parse("29/04/2024 10:00:00"),
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            }
        };
        int seedsCount = appointmentEntities.Count();

        int deletedSeeds = appointmentEntities.RemoveAll(
            ass => _dbContext.Appointments.Any(a => a.Id == ass.Id)
        );

        if (deletedSeeds < seedsCount)
        {
            _dbContext.Appointments.AddRange(appointmentEntities);
            _dbContext.SaveChanges(); // blocking
            return true;
        }

        return false;
    }

    public bool SeedAppointmentUsers()
    {
        List<AppointmentUserEntity> entities = new()
        {
            new AppointmentUserEntity()
            {
                AppointmentEntityId = SampleAppointment1Id,
                UserEntityId = Employee1UserId,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            },
            new AppointmentUserEntity()
            {
                AppointmentEntityId = SampleAppointment1Id,
                UserEntityId = Customer1UserId,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            },
            new AppointmentUserEntity()
            {
                AppointmentEntityId = SampleAppointment2Id,
                UserEntityId = Employee1UserId,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            },
            new AppointmentUserEntity()
            {
                AppointmentEntityId = SampleAppointment2Id,
                UserEntityId = Customer2UserId,
                IsDeleted = false,
                CreatedDate = DateTime.Now
            }
        };
        int seedCount = entities.Count();

        int deletedSeeds = entities.RemoveAll(
            e => _dbContext.AppointmentUsers
                .Any(au => au.AppointmentEntityId == e.AppointmentEntityId && au.UserEntityId == e.UserEntityId)
        );

        if (deletedSeeds < seedCount)
        {
            _dbContext.AppointmentUsers.AddRange(entities);
            _dbContext.SaveChanges();

            return true;
        }

        return false;
    }
}