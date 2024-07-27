using AdminPanel.BuildingConfiguration.Query.Application.DTOs;
using AdminPanel.Shared.DTOs;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Queries;

public record GetBuildingByIdQuery(Guid Id) : IRequest<Response<BuildingDto>>;