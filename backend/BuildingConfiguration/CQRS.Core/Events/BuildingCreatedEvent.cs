using CQRS.Core.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS.Core.Events;

public class BuildingCreatedEvent : BaseEvent
{
    public BuildingCreatedEvent() : base(nameof(BuildingCreatedEvent))
    {
    }
    
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }
    public int ConstructionTime { get; set; }
}