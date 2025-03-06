using System.Globalization;
using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Service.IntegrationEvents.EventHandlers;

public class ProductPriceUpdatedEventHandler(IDistributedCache cache) : IEventHandler<ProductPriceUpdatedEvent>
{
    private readonly DistributedCacheEntryOptions _cacheEntryOptions = new()
    {
        SlidingExpiration = TimeSpan.FromHours(24)
    };

    public async Task Handle(ProductPriceUpdatedEvent @event)
    {
        var existingProductPrice = await cache.GetStringAsync(@event.ProductId.ToString());
        if (existingProductPrice is null || !string.Equals(existingProductPrice, @event.NewPrice.ToString(CultureInfo.InvariantCulture)))
        {
            await cache.SetStringAsync(@event.ProductId.ToString(), 
                @event.NewPrice.ToString(CultureInfo.InvariantCulture), _cacheEntryOptions);
        }
    }
}