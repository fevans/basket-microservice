using Basket.Service.Models;

namespace Basket.Service.Infrastructure.Data;


/// <summary>
/// In-memory implementation of the <see cref="IBasketStore"/> interface.
/// This class provides methods to manage customer baskets using an in-memory dictionary.
/// </summary>
internal class  InMemoryBasketStore : IBasketStore
{
    private static readonly Dictionary<string, CustomerBasket> Baskets = [];

    // Get the customer basket by customer ID
    public CustomerBasket GetBasketByCustomerId(string customerId)
        => Baskets.TryGetValue(customerId, out var basket) ? basket 
            : new CustomerBasket {CustomerId = customerId};
    
    // Create a new customer basket
    public void CreateCustomerBasket(CustomerBasket customerBasket) => Baskets[customerBasket.CustomerId] = customerBasket;
    
    // Update the customer basket if it exists, otherwise create a new customer basket
    public void UpdateCustomerBasket(CustomerBasket customerBasket)
    {
        if (Baskets.TryGetValue(customerBasket.CustomerId, out _))
        {
            Baskets[customerBasket.CustomerId] = customerBasket;
        }
        else
        {
            CreateCustomerBasket(customerBasket);
        }
    }

    // Delete the customer basket by customer ID
    public void DeleteCustomerBasket(string customerId) => Baskets.Remove(customerId);
}