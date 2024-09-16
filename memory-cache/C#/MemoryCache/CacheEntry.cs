namespace MemoryCache;

public class CacheEntry<TValue>
{
    public TValue Value { get; }
    public DateTime ExpiryTime { get; }

    public CacheEntry(TValue value, DateTime expiryTime)
    {
        Value = value;
        ExpiryTime = expiryTime;
    }
}
