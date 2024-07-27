using AdminPanel.BuildingConfiguration.Command.Domain.Enums;
using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Notifications;

public class BuildingCreatedEvent : INotification
{
    public Guid Id { get; set; }

    public BuildingType BuildingType { get; set; }

    public decimal BuildingCost { get; set; }

    public int ConstructionTime { get; set; }
}