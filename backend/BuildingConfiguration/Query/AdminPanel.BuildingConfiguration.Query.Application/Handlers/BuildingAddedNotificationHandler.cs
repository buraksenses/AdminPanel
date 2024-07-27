using AdminPanel.BuildingConfiguration.Query.Application.Notifications;
using AdminPanel.BuildingConfiguration.Query.Domain.Entities;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Handlers;

public class BuildingAddedNotificationHandler : INotificationHandler<BuildingCreatedNotification>
{
    private readonly IBuildingRepository _repository;

    public BuildingAddedNotificationHandler(IBuildingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(BuildingCreatedNotification notification, CancellationToken cancellationToken)
    {
        await _repository.CreateAsync(new Building
        {
            Id = notification.Id,
            BuildingCost = notification.BuildingCost,
            BuildingType = notification.BuildingType,
            ConstructionTime = notification.ConstructionTime
        });
    }
}