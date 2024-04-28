using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QAgencyAppointment.Business.Options;

namespace QAgencyAppointment.Business.Services;

public class TokenService
{
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        _jwtSecurityTokenHandler = new();
        _jwtOptions = jwtOptions;
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
        _signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);
    }
    
    public string GenerateJwtToken(IList<Claim> claims)
    {
        JwtSecurityToken jwtSecurityToken = new(
            _jwtOptions.Value.Issuer,
            _jwtOptions.Value.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(_jwtOptions.Value.DurationInMinutes),
            signingCredentials: _signingCredentials
        );

        return _jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
    }
}
