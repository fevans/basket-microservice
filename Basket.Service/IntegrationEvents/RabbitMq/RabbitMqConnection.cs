using RabbitMQ.Client;

namespace Basket.Service.IntegrationEvents.RabbitMq;

public class RabbitMqConnection : IRabbitMqConnection
{
    private IConnection? _connection;
    private readonly RabbitMqOptions _options;
    public IConnection Connection => _connection!;

    public  RabbitMqConnection(RabbitMqOptions options)
    {
        _options = options;
        InitializeConnection();
    }

    private Task InitializeConnection()
    {
        var factory = new ConnectionFactory { HostName = _options.HostName };
        // _connection = new ConnectionFactory { HostName = _options.HostName }.CreateConnection();
        _connection = factory.CreateConnection();
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _connection?.Dispose();
    }
}