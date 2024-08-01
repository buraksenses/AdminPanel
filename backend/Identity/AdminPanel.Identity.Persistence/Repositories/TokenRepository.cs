using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminPanel.Identity.Domain.Entities;
using AdminPanel.Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdminPanel.Identity.Persistence.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;
    private readonly CookieOptions _cookieOptions;

    public TokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(1)
        };
    }
    
    public (string token, CookieOptions cookieOptions) CreateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), _cookieOptions);
    }
}