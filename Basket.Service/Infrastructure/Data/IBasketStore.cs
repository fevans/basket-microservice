using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;


internal interface IBasketStore
{
    CustomerBasket GetBasketByCustomerId(string customerId);
    
    // Add Create customer basket method
    void CreateCustomerBasket(CustomerBasket customerBasket);
    
    // Add Update customer basket method
    
    void UpdateCustomerBasket(CustomerBasket customerBasket);
    
    // Add Delete customer basket method
    void DeleteCustomerBasket(string customerId);
    
}