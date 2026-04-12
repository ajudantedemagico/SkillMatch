using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using SkillMatch.Api.Models;

namespace SkillMatch.Api.Services;

public interface ITokenService
{
    string GenerateToken(Usuario usuario);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Usuario usuario)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"] ?? "SkillMatch";
        var jwtAudience = _configuration["Jwt:Audience"] ?? "SkillMatchApp";
        
        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT Key is not configured");

        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
