using Basket.Service.ApiModels;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.Service.Endpoints;

public static class BasketApiEndpoints
{
    private static async  Task<decimal> GetProductPriceAsync (IDistributedCache cache, string productId)
    {
        var isValid = decimal.TryParse( await cache.GetStringAsync(productId), out var cachedProductPrice);
        return isValid ? cachedProductPrice : 0.0m;
    }
    
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        // Add Get basket endpoint
        routeBuilder.MapGet("/{customerId}", GetBasketAsync);
        
        // Add Create basket endpoint
        routeBuilder.MapPost( "/{customerId}", 
            CreateBasketAsync);
        
        //Add Update basket endpoint
        routeBuilder.MapPut("/{customerId}", AddBasketProductAsync);
        
        // Add Delete basket product endpoint
        routeBuilder.MapDelete("/{customerId}/{productId}",  DeleteBasketProductAsync);
        
        // Add Delete basket endpoint
        routeBuilder.MapDelete("/{customerId}", DeleteBasketAsync);
    }

    internal static async Task<NoContent> DeleteBasketAsync([FromServices] IBasketStore basketStore, string customerId)
    {
        await basketStore.DeleteCustomerBasket(customerId);
        return TypedResults.NoContent();
    }

    internal static async Task<NoContent> DeleteBasketProductAsync([FromServices] IBasketStore basketStore, string customerId, string productId)
    {
        var customerBasket = await basketStore.GetBasketByCustomerId(customerId);
        customerBasket.RemoveBasketProduct(productId);
        await basketStore.UpdateCustomerBasket(customerBasket);
        return TypedResults.NoContent();
    }

    internal static async Task<NoContent> AddBasketProductAsync([FromServices] IBasketStore basketStore, [FromServices] IDistributedCache cache, string customerId, AddBasketProductRequest request)
    {
        var customerBasket = await basketStore.GetBasketByCustomerId(customerId);
        var cachedProductPrice = await GetProductPriceAsync(cache, request.ProductId);
        customerBasket.AddBasketProduct(new BasketProduct(request.ProductId, request.ProductName, cachedProductPrice, request.Quantity));
        await basketStore.UpdateCustomerBasket(customerBasket);
        return TypedResults.NoContent();
    }

    internal static async Task<Created> CreateBasketAsync([FromServices] IBasketStore basketStore, [FromServices] IDistributedCache cache, string customerId, CreateBasketRequest request)
    {
        var customerBasket = new CustomerBasket() { CustomerId = customerId };
        var cachedProductPrice = await GetProductPriceAsync(cache, request.ProductId);
        customerBasket.AddBasketProduct(new BasketProduct(request.ProductId, request.ProductName, cachedProductPrice));
        await basketStore.CreateCustomerBasket(customerBasket);
        return TypedResults.Created();
    }

    internal static async Task<CustomerBasket> GetBasketAsync([FromServices] IBasketStore basketStore, string customerId)
    {
        return await basketStore.GetBasketByCustomerId(customerId);
    }
}