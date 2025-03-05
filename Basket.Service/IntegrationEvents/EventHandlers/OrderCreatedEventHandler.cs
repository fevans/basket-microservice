using Basket.Service.Infrastructure.Data;
using ECommerce.Shared.Infrastructure.EventBus.Abstractions;

namespace Basket.Service.IntegrationEvents.EventHandlers;

internal class OrderCreatedEventHandler(IBasketStore basketStore) : IEventHandler<OrderCreatedEvent>
{
    public Task Handle(OrderCreatedEvent @event)
    {
        basketStore.DeleteCustomerBasket(@event.CustomerId);

        return Task.CompletedTask;
    }
}