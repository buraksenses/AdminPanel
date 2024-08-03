using AdminPanel.BuildingConfiguration.Query.Application.DTOs;
using AdminPanel.Shared.DTOs;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Queries;

public record GetAllBuildingsQuery : IRequest<Response<List<BuildingDto>>>;