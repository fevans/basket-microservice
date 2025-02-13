namespace Basket.Service.IntegrationEvents.RabbitMq;

public static class RabbitMqExtensions
{
    /// <summary>
    /// Adds the RabbitMQ event bus to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the event bus to.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqOptions = new RabbitMqOptions();
        configuration.GetSection(RabbitMqOptions.RabbitMqSectionName).Bind(rabbitMqOptions);
        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(rabbitMqOptions));
    }
}