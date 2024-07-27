using CQRS.Core.Events;

namespace EcoVerse.StockManagement.Common.Events;

public class InventoryItemPriceUpdatedEvent : BaseEvent
{
    public decimal Price { get; set; }
    
    public InventoryItemPriceUpdatedEvent() : base(nameof(InventoryItemPriceUpdatedEvent))
    {
    }
}