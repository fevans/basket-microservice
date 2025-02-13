using Basket.Service.ApiModels;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Service.Endpoints;

public static class BasketApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        // Add Get basket endpoint
        routeBuilder.MapGet("/{customerId}", ([FromServices] IBasketStore basketStore, string customerId) =>
        basketStore.GetBasketByCustomerId(customerId));
        
        // Add Create basket endpoint
        routeBuilder.MapPost("/{customerId}", ([FromServices] IBasketStore basketStore, string customerId, CreateBasketRequest request) =>
        {
            var customerBasket = new CustomerBasket() { CustomerId = customerId };
            customerBasket.AddBasketProduct(new BasketProduct(
                request.ProductId, request.ProductName));
            basketStore.CreateCustomerBasket(customerBasket);
            return TypedResults.Created();
        });
        
        //Add Update basket endpoint
        routeBuilder.MapPut("/{customerId}", ([FromServices] IBasketStore basketStore, string customerId, AddBasketProductRequest request) =>
        {
            var customerBasket = basketStore.GetBasketByCustomerId(customerId);
            customerBasket.AddBasketProduct(new BasketProduct(
                request.ProductId, request.ProductName, request.Quantity));
            basketStore.UpdateCustomerBasket(customerBasket);
            return TypedResults.NoContent();
        });
        
        // Add Delete basket product endpoint
        routeBuilder.MapDelete("/{customerId}/{productId}", ([FromServices] IBasketStore basketStore, string customerId, string productId) =>
        {
            var customerBasket = basketStore.GetBasketByCustomerId(customerId);
            customerBasket.RemoveBasketProduct(productId);
            basketStore.UpdateCustomerBasket(customerBasket);
            return TypedResults.NoContent();
        });
        
        // Add Delete basket endpoint
        routeBuilder.MapDelete("/{customerId}", ([FromServices] IBasketStore basketStore, string customerId) =>
        {
            basketStore.DeleteCustomerBasket(customerId);
            return TypedResults.NoContent();
        });
    }
}