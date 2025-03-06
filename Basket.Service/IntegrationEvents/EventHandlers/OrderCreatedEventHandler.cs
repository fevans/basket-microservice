using Basket.Service.Infrastructure.Data;
using ECommerce.Shared.Infrastructure.EventBus.Abstractions;

namespace Basket.Service.IntegrationEvents.EventHandlers;

internal class OrderCreatedEventHandler(IBasketStore basketStore) : IEventHandler<OrderCreatedEvent>
{
    public  async Task Handle(OrderCreatedEvent @event)
    {
       await basketStore.DeleteCustomerBasket(@event.CustomerId);

        //return Task.CompletedTask;
    }
}