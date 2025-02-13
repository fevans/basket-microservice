using RabbitMQ.Client;

namespace Basket.Service.IntegrationEvents.RabbitMq;


public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}