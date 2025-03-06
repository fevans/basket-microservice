using System.Text;
using Basket.Service.ApiModels;
using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Distributed;
using NSubstitute;

namespace Basket.Tests.Endpoints;

public class BasketApiEndpointsTests
{
    private readonly IBasketStore _basketStore = Substitute.For<IBasketStore>();
    private readonly IDistributedCache _cache = Substitute.For<IDistributedCache>();
    
    [Fact]
    public async Task GivenExistingBasket_WhenCallingGetBasket_ThenReturnsBasket()
    {
        // Arrange
        const string customerId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };
        _basketStore.GetBasketByCustomerId(customerId)
            .Returns(customerBasket);
        // Act
        var result = await BasketApiEndpoints.GetBasketAsync(_basketStore, customerId);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result.CustomerId);
    }

    
    
    [Fact]
    public async Task GivenExistingBasket_WhenCallingUpdateBasket_ThenUpdatesBasket()
    {
        // Arrange
        const string customerId = "1";
        const string productId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };
        _basketStore.GetBasketByCustomerId(customerId)
            .Returns(customerBasket);
        var updateBasketRequest = new AddBasketProductRequest(ProductId: productId, ProductName: "Test Name", Quantity: 2);
        _cache.GetAsync(productId).Returns(Encoding.UTF8.GetBytes("1.00"));

        // Act
        var result = await BasketApiEndpoints.AddBasketProductAsync(_basketStore, _cache, customerId, updateBasketRequest);

        // Assert
        Assert.NotNull(result);
        var updateResult = (NoContent)result;
        Assert.NotNull(updateResult);
        
        Assert.Equal(2, customerBasket.Products.FirstOrDefault()?.Quantity);
    }
    
    [Fact]
    public async Task GivenNewBasketRequest_WhenCallingCreateBasket_ThenCreatesBasket()
    {
        // Arrange
        const string customerId = "1";
        const string productId = "1";
        var createBasketRequest = new CreateBasketRequest(ProductId: productId, ProductName: "Test Name");
        _cache.GetAsync(productId).Returns(Encoding.UTF8.GetBytes("1.00"));

        // Act
        var result = await BasketApiEndpoints.CreateBasketAsync(_basketStore, _cache, customerId, createBasketRequest);

        // Assert
        Assert.NotNull(result);
        var createResult = (Created)result;
        Assert.NotNull(createResult);
        //Assert.Equal($"{customerId}/{productId}", createResult.Location);
    }
    
    [Fact]
    public async Task GivenExistingBasket_WhenCallingDeleteBasket_ThenDeletesBasket()
    {
        // Arrange
        const string customerId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };
        _basketStore.GetBasketByCustomerId(customerId)
            .Returns(customerBasket);
        // Act
        var result = await BasketApiEndpoints.DeleteBasketAsync(_basketStore, customerId);
        // Assert
        Assert.NotNull(result);
        var deleteResult = (NoContent)result;
        Assert.NotNull(deleteResult);
    }
    
    [Fact]
    public async Task GivenExistingBasket_WhenCallingDeleteBasketProduct_ThenDeletesProduct()
    {
        // Arrange
        const string customerId = "1";
        const string productId = "1";
        var customerBasket = new CustomerBasket { CustomerId = customerId };
        customerBasket.AddBasketProduct(new BasketProduct(productId, "Test Name", 1.00m));
        _basketStore.GetBasketByCustomerId(customerId)
            .Returns(customerBasket);
        // Act
        var result = await BasketApiEndpoints.DeleteBasketProductAsync(_basketStore, customerId, productId);
        // Assert
        Assert.NotNull(result);
        var deleteResult = (NoContent)result;
        Assert.NotNull(deleteResult);
        Assert.Empty(customerBasket.Products);
    }
}