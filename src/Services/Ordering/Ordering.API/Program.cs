using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config =>
{
    //var assembly = typeof(Program).Assembly;
    //config.AddConsumers(assembly);

    config.AddConsumer<BasketCheckoutConsumer>();
    config.SetKebabCaseEndpointNameFormatter();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ConfigureEndpoints(ctx);
        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});

// General Configuration
builder.Services.AddScoped<BasketCheckoutConsumer>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MigrateDatabase<OrderContext>((context, service) =>
{
    var logger = service.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed
        .SeedAsync(context, logger)
        .Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();