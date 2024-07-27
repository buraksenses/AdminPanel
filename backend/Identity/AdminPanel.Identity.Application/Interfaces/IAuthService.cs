using AdminPanel.Identity.Application.DTOs;
using AdminPanel.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Identity.Application.Interfaces;

public interface IAuthService
{
    Task<Response<IdentityResult>> RegisterUserAsync(RegisterRequestDto requestDto);

    Task<Response<LoginResponseDto>> LoginUserAsync(LoginRequestDto requestDto);
}