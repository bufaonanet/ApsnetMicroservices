using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingWebApp.Models;
using ShoppingWebApp.Services;

namespace ShoppingWebApp.Pages;

public class CheckOutModel : PageModel
{
    private readonly IBasketService _basketService;    

    public CheckOutModel(IBasketService basketService)
    {
        _basketService = basketService;
    }


    [BindProperty]
    public BasketCheckoutModel Order { get; set; }

    public BasketModel Cart { get; set; } = new();


    public async Task<IActionResult> OnGetAsync()
    {
        var userName = "swn";
        Cart = await _basketService.GetBasket(userName);

        return Page();
    }

    public async Task<IActionResult> OnPostCheckOutAsync()
    {
        var userName = "swn";
        Cart = await _basketService.GetBasket(userName);

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Order.UserName = userName;
        Order.TotalPrice = Cart.TotalPrice;

        await _basketService.CheckoutBasket(Order);

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}