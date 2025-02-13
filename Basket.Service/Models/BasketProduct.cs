namespace Basket.Service.Models;

internal record BasketProduct(string ProductId, string ProductName, int Quantity = 1);

    // define a customer basket model. This model will be used to store the basketProduct records