import pytest
import time
from memory_cache import MemoryCache

def test_given_empty_cache_when_item_added_then_item_can_be_retrieved():
    # Arrange
    cache = MemoryCache()
    key = 'key1'
    value = 'value1'

    # Act
    cache.set(key, value)
    retrieved_value = cache.get(key)
    missing_value = cache.get('key2')

    # Assert
    assert retrieved_value == value
    assert missing_value is None

def test_given_item_with_ttl_when_ttl_expires_then_item_is_removed():
    # Arrange
    cache = MemoryCache(ttl=1)
    key = 'key1'
    value = 'value1'
    cache.set(key, value)

    # Act
    time.sleep(1.1)
    retrieved_value = cache.get(key)

    # Assert
    assert retrieved_value is None

def test_given_cache_at_max_size_when_new_item_added_then_lru_item_is_evicted():
    # Arrange
    cache = MemoryCache(max_size=2)
    cache.set('key1', 'value1')
    cache.set('key2', 'value2')

    # Act
    cache.set('key3', 'value3')  # This should evict 'key1'
    value_key1 = cache.get('key1')
    value_key2 = cache.get('key2')
    value_key3 = cache.get('key3')

    # Assert
    assert value_key1 is None
    assert value_key2 == 'value2'
    assert value_key3 == 'value3'

def test_given_cache_when_item_accessed_then_item_becomes_most_recently_used():
    # Arrange
    cache = MemoryCache(max_size=2)
    cache.set('key2', 'value2')
    cache.set('key3', 'value3')

    # Act
    cache.get('key2')  # Access 'key2' to make it recently used
    cache.set('key4', 'value4')  # This should evict 'key3'
    value_key2 = cache.get('key2')
    value_key3 = cache.get('key3')
    value_key4 = cache.get('key4')

    # Assert
    assert value_key2 == 'value2'
    assert value_key3 is None
    assert value_key4 == 'value4'

def test_given_config_in_memory_when_cache_initialized_then_settings_are_applied():
    # Arrange
    config = {
        'ttl': 2,
        'max_size': 2,
        'value_type': int
    }
    cache = MemoryCache(config=config)
    cache.set('key1', '10')
    cache.set('key2', '20')

    # Act
    cache.set('key3', '30')  # Should evict 'key1'
    value_key1 = cache.get('key1')
    value_key2 = cache.get('key2')
    value_key3 = cache.get('key3')

    # Assert
    assert cache.ttl == 2
    assert cache.max_size == 2
    assert cache.value_type == int
    assert value_key1 is None
    assert value_key2 == 20
    assert value_key3 == 30

def test_given_no_config_when_cache_initialized_then_defaults_are_used():
    # Arrange
    cache = MemoryCache()

    # Act
    # No action needed

    # Assert
    assert cache.ttl == 60
    assert cache.max_size == 100
    assert cache.value_type == str

def test_given_negative_ttl_and_size_in_config_when_cache_initialized_then_defaults_are_used():
    # Arrange
    config = {
        'ttl': -10,
        'max_size': -5,
        'value_type': str
    }
    cache = MemoryCache(config=config)

    # Act
    # No action needed

    # Assert
    assert cache.ttl == 60  # Defaults to 60 due to negative TTL in config
    assert cache.max_size == 100  # Defaults to 100 due to negative max_size
    assert cache.value_type == str

def test_given_value_type_int_when_non_int_value_set_then_error_is_raised():
    # Arrange
    cache = MemoryCache(value_type=int)

    # Act & Assert
    cache.set('key1', '10')
    assert cache.get('key1') == 10

    with pytest.raises(ValueError):
        cache.set('key2', 'abc')  # Should raise ValueError

def test_given_custom_value_type_when_item_set_then_item_is_stored_correctly():
    # Arrange
    class MyAwesomeType:
        def __init__(self, value):
            self.value = value

        @classmethod
        def from_string(cls, value_str):
            return cls(value_str)

    config = {
        'value_type': 'MyAwesomeType'
    }
    custom_types = {
        'MyAwesomeType': MyAwesomeType
    }
    cache = MemoryCache(config=config, custom_types=custom_types)
    key = 'key1'
    value = 'custom_value'

    # Act
    cache.set(key, value)
    result = cache.get(key)

    # Assert
    assert isinstance(result, MyAwesomeType)
    assert result.value == value
