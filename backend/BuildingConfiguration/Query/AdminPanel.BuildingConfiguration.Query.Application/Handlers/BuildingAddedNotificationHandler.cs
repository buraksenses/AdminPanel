using AdminPanel.BuildingConfiguration.Query.Application.Notifications;
using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Handlers;

public class BuildingAddedNotificationHandler : INotificationHandler<BuildingCreatedEvent>
{
    private readonly IBuildingRepository _repository;

    public BuildingAddedNotificationHandler(IBuildingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(BuildingCreatedEvent @event, CancellationToken cancellationToken)
    {
        await _repository.CreateAsync(new Building
        {
            Id = @event.Id,
            BuildingCost = @event.BuildingCost,
            BuildingType = @event.BuildingType,
            ConstructionTime = @event.ConstructionTime
        });
    }
}