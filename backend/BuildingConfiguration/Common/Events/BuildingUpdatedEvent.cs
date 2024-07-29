using CQRS.Core.Events;

namespace Common.Events;

public class BuildingUpdatedEvent : BaseEvent
{
    public BuildingUpdatedEvent() : base(nameof(BuildingUpdatedEvent))
    {
    }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}