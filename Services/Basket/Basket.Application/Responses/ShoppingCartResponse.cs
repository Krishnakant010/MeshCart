namespace Basket.Application.Responses;

public class ShoppingCartResponse
{
    public string UserName { get; init; }
    public List<ShoppingCartItemResponse>Items { get; init; }

    public ShoppingCartResponse()
    {
        UserName = string.Empty;
        Items = new List<ShoppingCartItemResponse>();
    }

    public ShoppingCartResponse(string userName):this(userName,new List<ShoppingCartItemResponse>())
    {
        
    }

    private ShoppingCartResponse(string userName, List<ShoppingCartItemResponse> shoppingCartItemResponses)
    {
        this.UserName = userName;
        this.Items = shoppingCartItemResponses;
    }

    public decimal TotalPrice => Items.Sum(i => i.Price * i.Quantity);
}