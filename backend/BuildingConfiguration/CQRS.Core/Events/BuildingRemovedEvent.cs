namespace CQRS.Core.Events;

public class BuildingRemovedEvent : BaseEvent
{
    public BuildingRemovedEvent() : base(nameof(BuildingRemovedEvent))
    {
    }
}