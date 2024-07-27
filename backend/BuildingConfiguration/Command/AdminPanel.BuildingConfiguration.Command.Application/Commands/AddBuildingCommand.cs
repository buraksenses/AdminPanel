using AdminPanel.BuildingConfiguration.Command.Application.DTOs;
using AdminPanel.BuildingConfiguration.Command.Domain.Enums;
using AdminPanel.Shared.DTOs;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Command.Application.Commands;

public class AddBuildingCommand : IRequest<Response<AddBuildingDto>>
{
    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}