using ShoppingWebApp.Services;

namespace ShoppingWebApp;

public static class StartupHelperExtensions
{

    public static IServiceCollection AddHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        // add http client services
        services.AddHttpClient<ICatalogService, CatalogService>(c =>
           c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));
        services.AddHttpClient<IBasketService, BasketService>(c =>
           c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));
        services.AddHttpClient<IOrderService, OrderService>(c =>
           c.BaseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]));

        return services;
    }

    public static void UseAppConfigurations(this WebApplication app)
    {      
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();
    }  
}
