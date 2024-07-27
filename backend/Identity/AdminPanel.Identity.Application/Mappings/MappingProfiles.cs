using AdminPanel.Identity.Application.DTOs;
using AdminPanel.Identity.Domain.Entities;
using AutoMapper;

namespace AdminPanel.Identity.Application.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, RegisterRequestDto>().ReverseMap();
    }
}