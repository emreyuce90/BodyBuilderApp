using BodyBuilder.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Services {
    public class RedisService : IRedisService {
        private readonly IDistributedCache _distributedCache;

        public RedisService(IDistributedCache distributedCache) {
            _distributedCache = distributedCache;
        }
        
        public async Task<T?> GetAsync<T>(string key) {
            string? cachedValue = await _distributedCache.GetStringAsync(key);
            if (cachedValue is not null) {
                T? data = JsonConvert.DeserializeObject<T>(cachedValue);
                return data;
            }
            return default(T);
        }

        public async Task RemoveAsync(string key) {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task SetAsync<T>(string key, T value) {
            var data = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, data);
        }
    }
}
