using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace N8T.Infrastructure.Cache.Redis.Internals
{
    public class RedisCacheService : IRedisCacheService
    {
        private const string GetKeysLuaScript = "return redis.call('keys', ARGV[1])";

        private const string ClearCacheLuaScript =
            "for _,k in ipairs(redis.call('KEYS', ARGV[1])) do\n" +
            "    redis.call('DEL', k)\n" +
            "end";

        private readonly RedisCacheOptions _redisCacheOptions;

        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        public RedisCacheService(IOptions<RedisCacheOptions> redisCacheOptions)
        {
            _redisCacheOptions = redisCacheOptions.Value;
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.Connect(redisCacheOptions.Value.GetConnectionString()));
        }

        public ConnectionMultiplexer ConnectionMultiplexer => _lazyConnection.Value;

        public IDatabase Database
        {
            get
            {
                _connectionLock.Wait();

                try
                {
                    return ConnectionMultiplexer.GetDatabase();
                }
                finally
                {
                    _connectionLock.Release();
                }
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> func)
        {
            return await GetOrSetAsync(key, func,
                TimeSpan.FromSeconds(_redisCacheOptions.RedisDefaultSlidingExpirationInSecond));
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> func, TimeSpan expiration)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key cannot be null, empty, or only whitespace.");
            }

            var valueAsString = await Database.StringGetAsync(key);
            if (!string.IsNullOrEmpty(valueAsString))
            {
                return GetByteToObject<T>(valueAsString);
            }

            var value = await func();
            if (value != null)
            {
                await Database.StringSetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), expiration);
            }

            return value;
        }

        public async Task<T> HashGetOrSetAsync<T>(string key, string hashField, Func<Task<T>> func)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key cannot be null, empty, or only whitespace.");
            }

            if (string.IsNullOrWhiteSpace(hashField))
            {
                throw new ArgumentException("HashField cannot be null, empty, or only whitespace.");
            }

            var keyWithPrefix = $"{_redisCacheOptions.Prefix}:{key}";
            var redisValue = await Database.HashGetAsync(keyWithPrefix, hashField.ToLower());
            if (!string.IsNullOrEmpty(redisValue))
            {
                return GetByteToObject<T>(redisValue);
            }

            var value = await func();
            if (value != null)
            {
                await Database.HashSetAsync(keyWithPrefix, hashField.ToLower(),
                    JsonSerializer.SerializeToUtf8Bytes(value));
            }

            return value;
        }

        public async Task<bool> RemoveAllKeysAsync(string pattern = "*")
        {
            var succeed = true;
            var keys = await GetKeysAsync($"{_redisCacheOptions.Prefix}:{pattern}");

            foreach (var key in keys)
            {
                succeed = await Database.KeyDeleteAsync(key);
            }

            return succeed;
        }

        public async Task RemoveAsync(string key)
        {
            var keyWithPrefix = $"{_redisCacheOptions.Prefix}:{key}";
            await Database.KeyDeleteAsync(keyWithPrefix);
        }

        public async Task ResetAsync()
        {
            await Database.ScriptEvaluateAsync(
                ClearCacheLuaScript,
                values: new RedisValue[] { _redisCacheOptions.Prefix + "*" });
        }

        public async Task<IEnumerable<string>> GetKeysAsync(string pattern)
        {
            var result = await Database.ScriptEvaluateAsync(
                GetKeysLuaScript,
                values: new RedisValue[] { pattern });

            return ((RedisResult[])result)
                .Where(x => x.ToString()!.StartsWith(_redisCacheOptions.Prefix))
                .Select(x => x.ToString())
                .ToArray();
        }

        public async Task<IEnumerable<T>> GetValuesAsync<T>(string key)
        {
            var keyWithPrefix = $"{_redisCacheOptions.Prefix}:{key}";
            var items = await Database.HashGetAllAsync(keyWithPrefix);
            return items.Select(x => GetByteToObject<T>(x.Value));
        }

        private static T GetByteToObject<T>(RedisValue value)
        {
            var readOnlySpan = new ReadOnlySpan<byte>(value);
            var obj = JsonSerializer.Deserialize<T>(readOnlySpan);
            return obj;
        }
    }
}