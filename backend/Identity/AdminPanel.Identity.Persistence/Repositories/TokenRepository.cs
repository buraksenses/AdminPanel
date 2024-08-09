using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AdminPanel.Identity.Domain;
using AdminPanel.Identity.Domain.Entities;
using AdminPanel.Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AdminPanel.Identity.Persistence.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly CookieOptions _cookieOptions;

    public TokenRepository(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddMinutes(60)
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
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), _cookieOptions);
    }

    public async Task<((string token, CookieOptions cookieOptions), RefreshToken newRefreshToken)> CreateRefreshToken(string? token)
    {
        var user = await _userRepository.GetUserByRefreshToken(token);
        if (user == null) throw new UnauthorizedAccessException("Invalid token");

        var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
        if (!refreshToken.IsActive) throw new UnauthorizedAccessException("Invalid token");

        
        var newJwtToken = CreateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        
        _userRepository.RevokeRefreshToken(refreshToken);
        await _userRepository.AddRefreshToken(user, newRefreshToken);
        await _userRepository.UpdateUserAsync(user);

        return (newJwtToken, newRefreshToken);
    }


    public static RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomNumber),
            Expires = DateTime.UtcNow.AddDays(20),
            Created = DateTime.UtcNow
        };
    }
}