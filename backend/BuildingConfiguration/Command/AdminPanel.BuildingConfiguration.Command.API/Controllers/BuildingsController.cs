using AdminPanel.BuildingConfiguration.Command.Application.Commands;
using AdminPanel.Shared.ControllerBases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.BuildingConfiguration.Command.Controllers;

public class BuildingsController : CustomBaseController
{
    private readonly IMediator _mediator;

    public BuildingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddBuildingAsync(AddBuildingCommand command)
    {
        var response = await _mediator.Send(command);
        return CreateActionResultInstance(response);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveBuildingAsync(Guid id)
    {
        var command = new RemoveBuildingCommand(id);
        var response = await _mediator.Send(command);
        return CreateActionResultInstance(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateBuildingAsync(UpdateBuildingCommand command)
    {
        var response = await _mediator.Send(command);
        return CreateActionResultInstance(response);
    }
}