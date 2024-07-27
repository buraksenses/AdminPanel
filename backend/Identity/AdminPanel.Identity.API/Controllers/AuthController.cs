using AdminPanel.Identity.Application.DTOs;
using AdminPanel.Identity.Application.Interfaces;
using AdminPanel.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Identity.API.Controllers;

public class AuthController : CustomBaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync(LoginRequestDto requestDto)
    {
        var response = await _authService.LoginUserAsync(requestDto);

        return CreateActionResultInstance(response);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequestDto requestDto)
    {
        var result = await _authService.RegisterUserAsync(requestDto);

        return CreateActionResultInstance(result);
    }
}