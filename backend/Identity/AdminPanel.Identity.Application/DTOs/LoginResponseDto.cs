using Microsoft.AspNetCore.Http;

namespace AdminPanel.Identity.Application.DTOs;

public record LoginResponseDto(string token, CookieOptions CookieOptions);