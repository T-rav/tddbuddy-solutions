namespace MemoryCache;

public class CacheConfig<TValue>
{
    public int MaxSize { get; set; } = 100;
    public TimeSpan Ttl { get; set; } = TimeSpan.FromSeconds(60);
    // Additional configurations can be added here

    public CacheConfig() { }

    public CacheConfig(int maxSize, TimeSpan ttl)
    {
        MaxSize = maxSize > 0 ? maxSize : 100;
        Ttl = ttl > TimeSpan.Zero ? ttl : TimeSpan.FromSeconds(60);
    }
}
