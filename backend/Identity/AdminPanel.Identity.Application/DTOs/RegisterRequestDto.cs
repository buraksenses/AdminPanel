namespace AdminPanel.Identity.Application.DTOs;

public record RegisterRequestDto(string username, string password, List<string> roles);