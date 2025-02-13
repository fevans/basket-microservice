namespace Basket.Service.Models;

internal record CustomerBasket {
    private readonly HashSet<BasketProduct> _products = [];
    public IEnumerable<BasketProduct> Products => _products;

    public required string CustomerId { get; init; }


// add a basket product to the basket. If the product already exists, remove the existing product and add the new product
// Required behavior as we are using a Hashet to store the products
    public void AddBasketProduct(BasketProduct basketProduct) {
        ArgumentNullException.ThrowIfNull(basketProduct);
        var existingProduct = _products.FirstOrDefault(p => p.ProductId == basketProduct.ProductId);
        if (existingProduct is not null) {
            _products.Remove(existingProduct);
            _products.Add(basketProduct);
        } else {
            _products.Add(basketProduct);
        }
    }
    // remove a basket product from the basket
    public void RemoveBasketProduct(string productId) => _products.RemoveWhere(p => p.ProductId == productId);        
    
}