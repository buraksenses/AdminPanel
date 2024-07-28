using CQRS.Core.Events;

namespace Common.Events;

public class BuildingRemovedEvent : BaseEvent
{
    public BuildingRemovedEvent() : base(nameof(BuildingRemovedEvent))
    {
    }
}