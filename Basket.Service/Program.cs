using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Infrastructure.Data.Redis;
using Basket.Service.IntegrationEvents;
using Basket.Service.IntegrationEvents.EventHandlers;
using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqSubscriberService(builder.Configuration)
    .AddEventHander<OrderCreatedEvent, OrderCreatedEventHandler>()
    .AddEventHander<ProductPriceUpdatedEvent, ProductPriceUpdatedEventHandler>();

builder.Services.AddScoped<IBasketStore, RedisBasketStore>()
    .AddRedisCache(builder.Configuration);


var app = builder.Build();
app.RegisterEndpoints();
app.Run();