using ECommerce.Shared.Infrastructure.EventBus;

namespace Basket.Service.IntegrationEvents;

public abstract record ProductPriceUpdatedEvent(int ProductId, decimal NewPrice) : Event;
