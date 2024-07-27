using AdminPanel.BuildingConfiguration.Query.Application.Queries;
using AdminPanel.Shared.ControllerBases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.BuildingConfiguration.Query.API.Controllers;

public class BuildingsController : CustomBaseController
{
    private readonly IMediator _mediator;

    public BuildingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var response = await _mediator.Send(new GetBuildingByIdQuery(id));
        return CreateActionResultInstance(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllBuildingsAsync()
    {
        var response = await _mediator.Send(new GetAllBuildingsQuery());
        return CreateActionResultInstance(response);
    }
}