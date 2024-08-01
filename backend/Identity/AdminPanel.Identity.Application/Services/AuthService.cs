using AdminPanel.Identity.Application.DTOs;
using AdminPanel.Identity.Application.Interfaces;
using AdminPanel.Identity.Application.Mappings;
using AdminPanel.Identity.Domain.Entities;
using AdminPanel.Identity.Domain.Interfaces;
using AdminPanel.Shared.DTOs;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Identity.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public AuthService(UserManager<User> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }


    public async Task<Response<IdentityResult>> RegisterUserAsync(RegisterRequestDto requestDto)
    {
        var user = ObjectMapper.Mapper.Map<User>(requestDto);
        user.Email = requestDto.username;
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
        var response = new LoginResponseDto(token, cookieOptions);

        return Response<LoginResponseDto>.Success(response, 200);
    }
}