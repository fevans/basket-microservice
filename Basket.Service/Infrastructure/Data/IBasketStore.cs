using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;


internal interface IBasketStore
{
    Task<CustomerBasket> GetBasketByCustomerId(string customerId);
    
    // Add Create customer basket method
    Task CreateCustomerBasket(CustomerBasket customerBasket);
    
    // Add Update customer basket method
    
    Task UpdateCustomerBasket(CustomerBasket customerBasket);
    
    // Add Delete customer basket method
    Task DeleteCustomerBasket(string customerId);
    
}