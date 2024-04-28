using System.Text;
using Microsoft.AspNetCore.Identity;
using QAgencyAppointment.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using QAgencyAppointment.Business.Interface;
using QAgencyAppointment.Business.Options;
using QAgencyAppointment.Business.Services;
using QAgencyAppointment.DataAccess.Interfaces;
using QAgencyAppointment.DataAccess.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using QAgencyAppointment.DataAccess.Seedings;

var builder = WebApplication.CreateBuilder(args);

// Registers configuration/options from appsettings.json
builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.OptionsName));
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Database context and identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=Database.db");
    options.EnableSensitiveDataLogging();
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Registers repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Data seeding
builder.Services.AddScoped<ISeeding, DataSeeding>(); 

// Registers business logic
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// Authentication and authorization
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes("Q2VsbEN1bHR1cmVTdWl0ZUFQSQf35Ha2gh");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "QAgencyAppointment",
            ValidAudience = "*",
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });
// builder.Services.AddAuthorization();

// Add swagger and API Controller
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    OpenApiSecurityScheme securityScheme = new()
    {
        Type = SecuritySchemeType.ApiKey,
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { securityScheme, Array.Empty<string>() }
    });

    options.CustomSchemaIds(type => type.ToString());
});


builder.Services.AddControllers();

var app = builder.Build();

// -----------------------------------------------------------------------------


// Migrate EFCore from previously create EFCore migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var seeding = scope.ServiceProvider.GetRequiredService<ISeeding>();
    
    db.Database.Migrate(); // blocking, db needs to be migrated before starting up the webapi
    
    // Seed user-related data
    seeding.SeedRoles();
    seeding.SeedUsers();
    seeding.SeedUserRoles();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoint =>
{
    endpoint.MapControllers();
});

app.Run();
