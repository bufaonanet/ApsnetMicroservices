
using Basket.API.Repositories.Interfaces;
using Basket.API.Repositories;
using Basket.API.GrpcServices;
using Discount.gRPC.Protos;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Redis Configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration
    .GetValue<string>("CacheSettings:ConnectionString");
});

// Grpc Configuration
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
            (o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ConfigureEndpoints(ctx);
    });
});

//General Configuration
builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();