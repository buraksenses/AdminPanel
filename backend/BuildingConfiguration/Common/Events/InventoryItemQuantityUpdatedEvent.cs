using CQRS.Core.Events;

namespace EcoVerse.StockManagement.Common.Events;

public class InventoryItemQuantityUpdatedEvent : BaseEvent
{
    public int Quantity { get; set; }

    public InventoryItemQuantityUpdatedEvent() : base(nameof(InventoryItemQuantityUpdatedEvent))
    {
    }
}