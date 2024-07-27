using MediatR;

namespace AdminPanel.BuildingConfiguration.Query.Application.Notifications;

public class BuildingRemovedEvent : INotification
{
    public Guid Id { get; set; }
}