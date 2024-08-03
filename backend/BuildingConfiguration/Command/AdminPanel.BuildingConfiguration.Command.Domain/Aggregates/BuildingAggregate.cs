using CQRS.Core.Domain;
using CQRS.Core.Domain.Enums;
using CQRS.Core.Events;

namespace AdminPanel.BuildingConfiguration.Command.Domain.Aggregates;

public class BuildingAggregate : AggregateRoot
{
    private BuildingType _buildingType;

    private decimal _buildingCost;

    private int _constructionTime;

    public BuildingAggregate()
    {
        
    }
    
    public BuildingAggregate(Guid id, BuildingType buildingType, decimal buildingCost, int constructionTime)
    {
        RaiseEvent(new BuildingCreatedEvent
        {
            Id = id,
            BuildingType = buildingType,
            BuildingCost = buildingCost,
            ConstructionTime = constructionTime
        });        
    }

    public void Apply(BuildingCreatedEvent @event)
    {
        _id = @event.Id;
        _buildingCost = @event.BuildingCost;
        _buildingType = @event.BuildingType;
        _constructionTime = @event.ConstructionTime;
    }

    public void RemoveBuilding(Guid buildingId)
    {
        RaiseEvent(new BuildingRemovedEvent
        {
            Id = buildingId
        });
    }

    public void Apply(BuildingRemovedEvent @event)
    {
        _id = @event.Id;
    }

    public void UpdateBuilding(BuildingUpdatedEvent @event)
    {
        RaiseEvent(new BuildingUpdatedEvent
        {
            BuildingCost = @event.BuildingCost,
            ConstructionTime = @event.ConstructionTime,
            Id = @event.Id
        });
    }
    
    public void Apply(BuildingUpdatedEvent @event)
    {
        _id = @event.Id;
        _buildingCost = @event.BuildingCost;
        _constructionTime = @event.ConstructionTime;
    }
}