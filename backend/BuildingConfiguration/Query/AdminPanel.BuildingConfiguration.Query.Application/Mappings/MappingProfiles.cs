using AdminPanel.BuildingConfiguration.Query.Application.DTOs;
using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AutoMapper;

namespace AdminPanel.BuildingConfiguration.Query.Application.Mappings;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Building, BuildingDto>().ReverseMap();
    }
}