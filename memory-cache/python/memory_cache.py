import time
from collections import OrderedDict

class CacheEntry:
    def __init__(self, value, expiry_time):
        self.value = value
        self.expiry_time = expiry_time

class MemoryCache:
    def __init__(self, max_size=100, ttl=60, value_type=str, config=None, custom_types=None):
        self.cache = OrderedDict()
        self.max_size = max_size
        self.ttl = ttl
        self.value_type = value_type
        self.custom_types = custom_types or {}
        if config:
            self.load_config(config)

    def load_config(self, config):
        ttl = config.get('ttl', self.ttl)
        max_size = config.get('max_size', self.max_size)
        value_type = config.get('value_type', self.value_type)

        # Validate TTL and max_size
        if ttl < 0:
            print("TTL cannot be negative. Using default TTL.")
            ttl = self.ttl
        if max_size < 0:
            print("Max size cannot be negative. Using default max_size.")
            max_size = self.max_size

        # Get value type
        if isinstance(value_type, type):
            self.value_type = value_type
        elif isinstance(value_type, str):
            self.value_type = self.get_type_from_string(value_type)
        else:
            raise ValueError(f"Invalid value type: {value_type}")

        self.ttl = ttl
        self.max_size = max_size

    def get_type_from_string(self, type_str):
        try:
            if type_str in ('str', 'int', 'float', 'bool'):
                return eval(type_str)
            elif type_str in self.custom_types:
                return self.custom_types[type_str]
            else:
                # For custom types, look in globals()
                return globals()[type_str]
        except Exception:
            raise ValueError(f"Invalid value type: {type_str}")

    def set(self, key, value):
        if not isinstance(key, str):
            raise ValueError("Key must be a string")

        # Convert value to the required type
        try:
            if hasattr(self.value_type, 'from_string'):
                value = self.value_type.from_string(value)
            else:
                value = self.value_type(value)
        except Exception:
            raise ValueError(f"Value cannot be converted to {self.value_type.__name__}")

        current_time = time.time()
        expiry_time = current_time + self.ttl

        if key in self.cache:
            self.cache.move_to_end(key)
            self.cache[key] = CacheEntry(value, expiry_time)
        else:
            self.cache[key] = CacheEntry(value, expiry_time)
            if len(self.cache) > self.max_size:
                self.cache.popitem(last=False)

    def get(self, key):
        if key in self.cache:
            entry = self.cache[key]
            current_time = time.time()
            if current_time > entry.expiry_time:
                del self.cache[key]
                return None
            else:
                self.cache.move_to_end(key)
                return entry.value
        else:
            return None
