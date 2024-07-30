using CQRS.Core.Domain.Enums;
using CQRS.Core.Events;

namespace Common.Events;

public class BuildingCreatedEvent : BaseEvent
{
    public BuildingCreatedEvent() : base(nameof(BuildingCreatedEvent))
    {
    }
    
    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}