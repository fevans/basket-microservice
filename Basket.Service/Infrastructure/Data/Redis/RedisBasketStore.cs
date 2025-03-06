using System.Text.Json;
using Basket.Service.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Service.Infrastructure.Data.Redis;

internal class RedisBasketStore (IDistributedCache cache): IBasketStore
{
    private readonly DistributedCacheEntryOptions _cacheEntryOptions = new()
    {
        SlidingExpiration = TimeSpan.FromHours(24)
    };
    
    public async Task<CustomerBasket> GetBasketByCustomerId(string customerId)
    {
        CustomerBasket customerBasket = new() { CustomerId = customerId };
        var cachedBasketProducts = await cache.GetStringAsync(customerId);
        if (cachedBasketProducts is null)
        {
            return customerBasket;
        }
        
        var deserializedProducts = 
            JsonSerializer.Deserialize<CustomerBasketCacheModel>(cachedBasketProducts);
        if (deserializedProducts == null ) return customerBasket;
        foreach (var product in deserializedProducts.Products)
        {
            customerBasket.AddBasketProduct(product);
        }
        return customerBasket;
    }

    public async Task CreateCustomerBasket(CustomerBasket customerBasket)
    {
        var serializedBasketProducts = JsonSerializer.Serialize(
            new CustomerBasketCacheModel(customerBasket.Products.ToList()));
        await cache.SetStringAsync(customerBasket.CustomerId, 
            serializedBasketProducts, _cacheEntryOptions);
    }

    public async Task UpdateCustomerBasket(CustomerBasket customerBasket)
    {
        var cachedBasketProducts = await cache.GetStringAsync(customerBasket.CustomerId);
        if (cachedBasketProducts is not null)
        {
            var serializedBasketProducts = JsonSerializer.Serialize(
                new CustomerBasketCacheModel(customerBasket.Products.ToList()));
            await cache.SetStringAsync(customerBasket.CustomerId, serializedBasketProducts, _cacheEntryOptions);
        }
    }

    public async Task DeleteCustomerBasket(string customerId) => await cache.RemoveAsync(customerId);
  
}