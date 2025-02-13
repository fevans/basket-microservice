using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.IntegrationEvents.RabbitMq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IBasketStore, InMemoryBasketStore>();
builder.Services.AddRabbitMqEventBus(builder.Configuration);

var app = builder.Build();
app.RegisterEndpoints();
app.Run();