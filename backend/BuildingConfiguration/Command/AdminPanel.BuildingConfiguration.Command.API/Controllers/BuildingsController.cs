using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using AdminPanel.Shared.ControllerBases;
using CQRS.Core.Messages;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.BuildingConfiguration.Command.Controllers;

public class BuildingsController : CustomBaseController
{
    private readonly IMediator _mediator;

    public BuildingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddBuildingAsync(AddBuildingCommand command)
    {
        var response = await _mediator.Send(command);
        return CreateActionResultInstance(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveBuildingAsync(Guid id, RemoveBuildingCommand command)
    {
        command.BuildingId = id;
        var response = await _mediator.Send(command);
        return CreateActionResultInstance(response);
    }
}