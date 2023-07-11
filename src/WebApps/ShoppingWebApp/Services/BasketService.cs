using ShoppingWebApp.Extensions;
using ShoppingWebApp.Models;

namespace ShoppingWebApp.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _client;

    public BasketService(HttpClient client)
    {
        _client = client;
    }

    public async Task<BasketModel> GetBasket(string userName)
    {
        var response = await _client.GetAsync($"/Basket/{userName}");
        return await response.ReadContentAs<BasketModel>();
    }

    public async Task<BasketModel> UpdateBasket(BasketModel model)
    {        
        var response = await _client.PostAsJson($"/Basket", model);
        return await response.ReadContentAs<BasketModel>();
    }

    public async Task CheckoutBasket(BasketCheckoutModel model)
    {        
        var response = await _client.PostAsJson($"/Basket/Checkout", model);
        response.EnsureSuccessStatusCode();
    }
}
