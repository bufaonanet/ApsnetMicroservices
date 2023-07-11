using ShoppingWebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddHttpClient(builder.Configuration)
    .AddRazorPages();

var app = builder.Build();

app.UseAppConfigurations();

app.Run();