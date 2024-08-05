using AdminPanel.Identity.Application.DTOs;
using AdminPanel.Identity.Application.Interfaces;
using AdminPanel.Identity.Application.Mappings;
using AdminPanel.Identity.Domain.Entities;
using AdminPanel.Identity.Domain.Interfaces;
using AdminPanel.Identity.Persistence.Repositories;
using AdminPanel.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Identity.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;

    public AuthService(UserManager<User> userManager, ITokenRepository tokenRepository, IUserRepository userRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }


    public async Task<Response<IdentityResult>> RegisterUserAsync(RegisterRequestDto requestDto)
    {
        var user = new User
        {
            Email = requestDto.username,
            UserName = requestDto.username
        };
        var result = await _userManager.CreateAsync(user, requestDto.password);

        if (!result.Succeeded)
        {
            return Response<IdentityResult>.Fail(result.Errors.Select(e => e.Description).ToList(), 400);
        }
        return Response<IdentityResult>.Success(result, 201);
    }

    public async Task<Response<LoginResponseDto>> LoginUserAsync(LoginRequestDto requestDto)
    {
        var user = await _userManager.FindByNameAsync(requestDto.username);

        if (user == null)
            return Response<LoginResponseDto>.Fail("User not found!", 404);
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, requestDto.password);

        if (!checkPasswordResult) 
            return Response<LoginResponseDto>.Fail("Username or password incorrect!", 404);

        var (token, cookieOptions) = _tokenRepository.CreateJwtToken(user);
        var refreshToken = TokenRepository.GenerateRefreshToken();

        await _userRepository.AddRefreshToken(user, refreshToken);
        await _userRepository.UpdateUserAsync(user);
        
        
        var response = new LoginResponseDto(token, refreshToken, cookieOptions);

        return Response<LoginResponseDto>.Success(response, 200);
    }

    public async Task<Response<RefreshTokenDto>> RefreshTokenAsync(string? token)
    {
        var data = await _tokenRepository.CreateRefreshToken(token);

        return Response<RefreshTokenDto>.Success(
            new RefreshTokenDto(data.Item1.token, data.newRefreshToken, data.Item1.cookieOptions), 200);
    }
}