using System.Text;
using System.Text.Json;
using Basket.Service.Infrastructure.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Basket.Service.IntegrationEvents.RabbitMq;

public class RabbitMqHostedService(IServiceProvider serviceProvider) : IHostedService
{
    // This method is called when the application stops
    public  Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    // This method is called when the application starts
    public  Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(() =>
        {
            var rabbitMQConnection = serviceProvider.GetRequiredService<IRabbitMqConnection>();
            var channel = rabbitMQConnection.Connection.CreateModel();
            channel.QueueDeclare(
                queue: nameof(OrderCreatedEvent),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnMessageReceived;
            channel.BasicConsume(
                queue: nameof(OrderCreatedEvent),
                autoAck: true,
                consumer: consumer);
        },
        TaskCreationOptions.LongRunning ) ;
        return Task.CompletedTask;
    }
    
    // This method is called when the consumer receives a message
    private void OnMessageReceived(object? sender, BasicDeliverEventArgs eventArgs)
    {
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        var @event = JsonSerializer.Deserialize(message, typeof(OrderCreatedEvent)) 
            as OrderCreatedEvent;
        using var scope = serviceProvider.CreateScope();
        var basketStore = scope.ServiceProvider.GetRequiredService<IBasketStore>();
        basketStore.DeleteCustomerBasket(@event.CustomerId);
    }
}