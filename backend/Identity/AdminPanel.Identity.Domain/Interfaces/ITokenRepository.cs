using AdminPanel.Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AdminPanel.Identity.Domain.Interfaces;

public interface ITokenRepository
{
    (string token, CookieOptions cookieOptions) CreateJwtToken(User user);
}