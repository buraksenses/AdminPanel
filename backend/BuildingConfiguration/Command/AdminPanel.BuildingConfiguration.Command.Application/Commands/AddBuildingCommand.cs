using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.Shared.DTOs;
using CQRS.Core.Domain.Enums;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Commands;

public class AddBuildingCommand : IRequest<Response<AddBuildingResponseDto>>
{
    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}