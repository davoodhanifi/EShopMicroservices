namespace Basket.Api.Models;

public class ShoppingCart
{
    public string Username { get; set; } = default!;
    public List<ShoppingCartItem> Items { get; set; } = [];
    public decimal TotalPrice => Items.Sum(i => i.Quantity * i.Price);

    public ShoppingCart(string username)
    {
        Username = username;
    }

    // Required for mapping
    public ShoppingCart()
    {
    }
}
