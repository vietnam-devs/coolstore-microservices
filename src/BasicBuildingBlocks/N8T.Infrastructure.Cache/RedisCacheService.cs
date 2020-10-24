using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace N8T.Infrastructure.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        private const string GetKeysLuaScript = "return redis.call('keys', ARGV[1])";
        private const string ClearCacheLuaScript =
            "for _,k in ipairs(redis.call('KEYS', ARGV[1])) do\n" +
            "    redis.call('DEL', k)\n" +
            "end";

        private readonly RedisCacheOptions _redisCacheOptions;

        private readonly AsyncLazy<ConnectionMultiplexer> _lazyConnection;

        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1, 1);

        public RedisCacheService(IOptions<RedisCacheOptions> redisCacheOptions)
        {
            _redisCacheOptions = redisCacheOptions.Value;

            // _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            //     ConnectionMultiplexer.Connect(redisCacheOptions.Value.GetConnectionString()));

            _lazyConnection = new AsyncLazy<ConnectionMultiplexer>(() =>
                ConnectionMultiplexer.ConnectAsync(redisCacheOptions.Value.GetConnectionString()));
        }

        // public ConnectionMultiplexer ConnectionMultiplexer => _lazyConnection.Value;
        //
        // public IDatabase Database
        // {
        //     get
        //     {
        //         _connectionLock.Wait();
        //
        //         try
        //         {
        //             return ConnectionMultiplexer.GetDatabase();
        //         }
        //         finally
        //         {
        //             _connectionLock.Release();
        //         }
        //     }
        // }

        public async Task<IDatabase> GetDatabaseAsync()
        {
            var conn = await _lazyConnection;
            return conn?.GetDatabase();
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

            var db = await GetDatabaseAsync();
            var valueAsString = await db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(valueAsString))
            {
                return GetByteToObject<T>(valueAsString);
            }

            var value = await func();
            if (value != null)
            {
                await db.StringSetAsync(key, JsonSerializer.SerializeToUtf8Bytes(value), expiration);
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

            var db = await GetDatabaseAsync();
            var keyWithPrefix = $"{_redisCacheOptions.Prefix}:{key}";
            var redisValue = await db.HashGetAsync(keyWithPrefix, hashField.ToLower());
            if (!string.IsNullOrEmpty(redisValue))
            {
                return GetByteToObject<T>(redisValue);
            }

            var value = await func();
            if (value != null)
            {
                await db.HashSetAsync(keyWithPrefix, hashField.ToLower(),
                    JsonSerializer.SerializeToUtf8Bytes(value));
            }

            return value;
        }

        public async Task<bool> RemoveAllKeysAsync(string pattern = "*")
        {
            var succeed = true;
            var keys = await GetKeysAsync($"{_redisCacheOptions.Prefix}:{pattern}");

            var db = await GetDatabaseAsync();
            foreach (var key in keys)
            {
                succeed = await db.KeyDeleteAsync(key);
            }

            return succeed;
        }

        public async Task RemoveAsync(string key)
        {
            var keyWithPrefix = $"{_redisCacheOptions.Prefix}:{key}";

            var db = await GetDatabaseAsync();
            await db.KeyDeleteAsync(keyWithPrefix);
        }

        public async Task ResetAsync()
        {
            var db = await GetDatabaseAsync();

            await db.ScriptEvaluateAsync(
                ClearCacheLuaScript,
                values: new RedisValue[] {_redisCacheOptions.Prefix + "*"});
        }

        public async Task<IEnumerable<string>> GetKeysAsync(string pattern)
        {
            var db = await GetDatabaseAsync();

            var result = await db.ScriptEvaluateAsync(
                GetKeysLuaScript,
                values: new RedisValue[] {pattern});

            return ((RedisResult[])result)
                .Where(x => x.ToString().StartsWith(_redisCacheOptions.Prefix))
                .Select(x => x.ToString())
                .ToArray();
        }

        private static T GetByteToObject<T>(RedisValue value)
        {
            var readOnlySpan = new ReadOnlySpan<byte>(value);
            var obj = JsonSerializer.Deserialize<T>(readOnlySpan);
            return obj;
        }
    }

    public sealed class AsyncLazy<T>
    {
        private readonly Lazy<Task<T>> _instance;
    
        public AsyncLazy(Func<T> factory)
        {
            _instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }
    
        public AsyncLazy(Func<Task<T>> factory)
        {
            _instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }
    
        public TaskAwaiter<T> GetAwaiter()
        {
            return _instance.Value.GetAwaiter();
        }
    
        public void Start() => _ = _instance.Value;
    }
}
