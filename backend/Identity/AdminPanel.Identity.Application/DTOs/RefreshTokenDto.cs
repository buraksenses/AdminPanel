using Microsoft.AspNetCore.Http;

namespace AdminPanel.Identity.Application.DTOs;

public record RefreshTokenDto(string AccessToken, string RefreshToken, CookieOptions CookieOptions);