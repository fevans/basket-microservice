namespace Basket.Service.Infrastructure.EventBus;

public record Event
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Guid Id { get; set; } = Guid.NewGuid();
}