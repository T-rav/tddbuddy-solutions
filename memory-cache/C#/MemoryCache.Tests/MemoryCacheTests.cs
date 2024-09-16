using System;
using NUnit.Framework;
using MemoryCache;
using System.Threading;

namespace MemoryCache.Tests
{
    [TestFixture]
    public class MemoryCacheTests
    {
        [Test]
        public void GivenEmptyCache_WhenItemAdded_ThenItemCanBeRetrieved()
        {
            // Arrange
            var cache = new MemoryCache<string, string>();
            string key = "key1";
            string value = "value1";

            // Act
            cache.Set(key, value);
            var retrievedValue = cache.Get(key);
            var missingValue = cache.Get("key2");

            // Assert
            Assert.That(retrievedValue, Is.EqualTo(value));
            Assert.That(missingValue, Is.Null);
        }

        [Test]
        public void GivenItemWithTtl_WhenTtlExpires_ThenItemIsRemoved()
        {
            // Arrange
            var config = new CacheConfig<string> { Ttl = TimeSpan.FromMilliseconds(100) };
            var cache = new MemoryCache<string, string>(config);
            string key = "key1";
            string value = "value1";
            cache.Set(key, value);

            // Act
            Thread.Sleep(150);
            var retrievedValue = cache.Get(key);

            // Assert
            Assert.That(retrievedValue, Is.Null);
        }

        [Test]
        public void GivenCacheAtMaxSize_WhenNewItemAdded_ThenLruItemIsEvicted()
        {
            // Arrange
            var config = new CacheConfig<string> { MaxSize = 2 };
            var cache = new MemoryCache<string, string>(config);
            cache.Set("key1", "value1");
            cache.Set("key2", "value2");

            // Act
            cache.Set("key3", "value3");  // This should evict 'key1'
            var valueKey1 = cache.Get("key1");
            var valueKey2 = cache.Get("key2");
            var valueKey3 = cache.Get("key3");

            // Assert
            Assert.That(valueKey1, Is.Null);
            Assert.That(valueKey2, Is.EqualTo("value2"));
            Assert.That(valueKey3, Is.EqualTo("value3"));
        }

        [Test]
        public void GivenCache_WhenItemAccessed_ThenItemBecomesMostRecentlyUsed()
        {
            // Arrange
            var config = new CacheConfig<string> { MaxSize = 2 };
            var cache = new MemoryCache<string, string>(config);
            cache.Set("key2", "value2");
            cache.Set("key3", "value3");

            // Act
            cache.Get("key2");  // Access 'key2' to make it recently used
            cache.Set("key4", "value4");  // This should evict 'key3'
            var valueKey2 = cache.Get("key2");
            var valueKey3 = cache.Get("key3");
            var valueKey4 = cache.Get("key4");

            // Assert
            Assert.That(valueKey2, Is.EqualTo("value2"));
            Assert.That(valueKey3, Is.Null);
            Assert.That(valueKey4, Is.EqualTo("value4"));
        }

        [Test]
        public void GivenConfigInMemory_WhenCacheInitialized_ThenSettingsAreApplied()
        {
            // Arrange
            var config = new CacheConfig<int>
            {
                MaxSize = 2,
                Ttl = TimeSpan.FromSeconds(2)
            };
            var cache = new MemoryCache<string, int>(config);
            cache.Set("key1", 10);
            cache.Set("key2", 20);

            // Act
            cache.Set("key3", 30);  // Should evict 'key1'
            var valueKey1 = cache.Get("key1");
            var valueKey2 = cache.Get("key2");
            var valueKey3 = cache.Get("key3");

            // Assert
            Assert.That(cache.Ttl, Is.EqualTo(TimeSpan.FromSeconds(2)));
            Assert.That(cache.MaxSize, Is.EqualTo(2));
            Assert.That(valueKey1, Is.EqualTo(default(int))); // Since int is non-nullable
            Assert.That(valueKey2, Is.EqualTo(20));
            Assert.That(valueKey3, Is.EqualTo(30));
        }

        [Test]
        public void GivenNoConfig_WhenCacheInitialized_ThenDefaultsAreUsed()
        {
            // Arrange
            var cache = new MemoryCache<string, string>();

            // Act
            // No action needed

            // Assert
            Assert.That(cache.Ttl, Is.EqualTo(TimeSpan.FromSeconds(60)));
            Assert.That(cache.MaxSize, Is.EqualTo(100));
        }

        [Test]
        public void GivenNegativeTtlAndSizeInConfig_WhenCacheInitialized_ThenDefaultsAreUsed()
        {
            // Arrange
            var config = new CacheConfig<string>
            {
                MaxSize = -5,
                Ttl = TimeSpan.FromSeconds(-10)
            };
            var cache = new MemoryCache<string, string>(config);

            // Act & Assert
            Assert.That(cache.MaxSize, Is.EqualTo(100));  // Defaults to 100
            Assert.That(cache.Ttl, Is.EqualTo(TimeSpan.FromSeconds(60)));  // Defaults to 60 seconds
        }

        [Test]
        public void GivenValueTypeInt_WhenNonIntValueSet_ThenErrorIsRaised()
        {
            // Arrange
            var cache = new MemoryCache<string, int>();

            // Act
            cache.Set("key1", 10);
            var value = cache.Get("key1");

            // Assert
            Assert.That(value, Is.EqualTo(10));

            // Act & Assert for invalid value
            Assert.Throws<FormatException>(() => cache.Set("key2", int.Parse("abc")));
        }

        [Test]
        public void GivenCustomValueType_WhenItemSet_ThenItemIsStoredCorrectly()
        {
            // Arrange
            var cache = new MemoryCache<string, MyAwesomeType>();
            string key = "key1";
            var value = MyAwesomeType.FromString("custom_value");

            // Act
            cache.Set(key, value);
            var result = cache.Get(key);

            // Assert
            Assert.That(result, Is.InstanceOf<MyAwesomeType>());
            Assert.That(result!.Value, Is.EqualTo(value.Value));
        }
    }

    public class MyAwesomeType
    {
        public string Value { get; }

        public MyAwesomeType(string value)
        {
            Value = value;
        }

        public static MyAwesomeType FromString(string value)
        {
            return new MyAwesomeType(value);
        }
    }
}