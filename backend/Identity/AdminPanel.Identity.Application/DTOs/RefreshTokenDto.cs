using AdminPanel.Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AdminPanel.Identity.Application.DTOs;

public record RefreshTokenDto(string AccessToken, RefreshToken RefreshToken, CookieOptions CookieOptions);