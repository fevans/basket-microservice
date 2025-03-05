namespace Basket.Service.Infrastructure.Data.Redis;

public class RedisOptions
{
    public const string RedisSectionName = "Redis";
    public string Configuration { get; set; } = null!;
}