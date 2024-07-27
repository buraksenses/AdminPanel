using AdminPanel.BuildingConfiguration.Query.Application.Notifications;
using AdminPanel.BuildingConfiguration.Query.Domain.Repositories;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Handlers;

public class BuildingRemovedNotificationHandler : INotificationHandler<BuildingRemovedEvent>
{
    private readonly IBuildingRepository _repository;

    public BuildingRemovedNotificationHandler(IBuildingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(BuildingRemovedEvent @event, CancellationToken cancellationToken)
    {
        var building = await _repository.GetByIdAsync(@event.Id);

        if (building == null)
            throw new Exception("Building not found!");

        await _repository.DeleteAsync(building);
    }
}