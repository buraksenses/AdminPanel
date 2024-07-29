using AdminPanel.BuildingConfiguration.Query.Application.DTOs;
using AdminPanel.BuildingConfiguration.Query.Application.Mappings;
using AdminPanel.BuildingConfiguration.Query.Application.Queries;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using AdminPanel.Shared.DTOs;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Handlers;

public class GetAllBuildingsQueryHandler : IRequestHandler<GetAllBuildingsQuery, Response<List<BuildingDto>>>
{
    private readonly IBuildingRepository _repository;

    public GetAllBuildingsQueryHandler(IBuildingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Response<List<BuildingDto>>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
    {
        var buildings = await _repository.GetAllAsync();
        var buildingsDto = ObjectMapper.Mapper.Map<List<BuildingDto>>(buildings);
        return Response<List<BuildingDto>>.Success(buildingsDto, 200);
    }
}