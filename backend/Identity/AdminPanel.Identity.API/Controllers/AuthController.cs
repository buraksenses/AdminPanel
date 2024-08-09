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
        
        Response.Cookies.Append("accessToken", response.Data.accessToken, response.Data.CookieOptions);
        Response.Cookies.Append("refreshToken", response.Data.refreshToken.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = response.Data.refreshToken.Expires
        });

        return CreateActionResultInstance(response);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequestDto requestDto)
    {
        var result = await _authService.RegisterUserAsync(requestDto);

        return CreateActionResultInstance(result);
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        var isExist = Request.Cookies.TryGetValue("refreshToken", out var value);
        if (!isExist)
            return BadRequest("Refresh token is null!");
        
        var response = await _authService.RefreshTokenAsync(value);
        
        Response.Cookies.Append("accessToken", response.Data.AccessToken, response.Data.CookieOptions);

        return CreateActionResultInstance(response);
    }
}