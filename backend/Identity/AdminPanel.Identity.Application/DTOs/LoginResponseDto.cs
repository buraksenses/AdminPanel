using AdminPanel.Identity.Domain;
using AdminPanel.Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace AdminPanel.Identity.Application.DTOs;

public record LoginResponseDto(string accessToken, RefreshToken refreshToken, CookieOptions CookieOptions);