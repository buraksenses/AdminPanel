using CQRS.Core.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace AdminPanel.BuildingConfiguration.Query.Domain.Entities;

public class Building
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public Guid Id { get; set; }

    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}