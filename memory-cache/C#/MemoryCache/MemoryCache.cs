using System;
using System.Collections.Generic;

namespace MemoryCache;

public class MemoryCache<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, CacheEntry<TValue>> _cache;
    private readonly LinkedList<TKey> _lruList;
    private readonly object _lock = new object();

    public int MaxSize { get; }
    public TimeSpan Ttl { get; }

    // Default constructor
    public MemoryCache() : this(new CacheConfig<TValue>()) { }

    // Constructor with CacheConfig
    public MemoryCache(CacheConfig<TValue> config)
    {
        MaxSize = config.MaxSize > 0 ? config.MaxSize : 100;
        Ttl = config.Ttl > TimeSpan.Zero ? config.Ttl : TimeSpan.FromSeconds(60);

        _cache = new Dictionary<TKey, CacheEntry<TValue>>();
        _lruList = new LinkedList<TKey>();
    }

    public void Set(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        lock (_lock)
        {
            if (_cache.ContainsKey(key))
            {
                _lruList.Remove(key);
            }
            else if (_cache.Count >= MaxSize)
            {
                // Evict the least recently used item
                TKey lruKey = _lruList.First!.Value;
                _lruList.RemoveFirst();
                _cache.Remove(lruKey);
            }

            DateTime expiryTime = DateTime.UtcNow.Add(Ttl);
            _cache[key] = new CacheEntry<TValue>(value, expiryTime);
            _lruList.AddLast(key);
        }
    }

    public TValue? Get(TKey key)
    {
        if (key == null)
            return default;

        lock (_lock)
        {
            if (_cache.TryGetValue(key, out CacheEntry<TValue> entry))
            {
                if (DateTime.UtcNow > entry.ExpiryTime)
                {
                    // Remove expired entry
                    _cache.Remove(key);
                    _lruList.Remove(key);
                    return default;
                }

                // Update LRU position
                _lruList.Remove(key);
                _lruList.AddLast(key);

                return entry.Value;
            }
            else
            {
                return default;
            }
        }
    }
}
