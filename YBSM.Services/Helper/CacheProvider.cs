using System;
using System.Net;
using System.Threading.Tasks;
using Core.Domain.Exceptions;
using Core.Domain.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Services.Helper
{
    public class CacheProvider
    {
        private readonly IDistributedCache _cache;

        public CacheProvider(IDistributedCache cache)
        {
            _cache = cache;
        }

  
        public async Task AddToCacheAsStringAsync(string key, string value, TimeSpan? span = null)
        {
            if (string.IsNullOrEmpty(key))
                throw new APIException(
                    "key is null...", HttpStatusCode.NotAcceptable);

            if (value is null)
                throw new APIException(
                    "value is null...", HttpStatusCode.NotAcceptable);


            if (span == null)
                await _cache.SetStringAsync(key, value);
            else
                await _cache.SetStringAsync(key, value,
                    new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = span});
        }

        public void InvalidateCache(string key)
        {
            if (!string.IsNullOrEmpty(key))
                _cache.Remove(key);
        }

        public async Task InvalidateCacheAsync(string key)
        {
            if (!string.IsNullOrEmpty(key))
                await _cache.RemoveAsync(key);
        }

        public string GetString(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new APIException(
                    "key is null...", HttpStatusCode.NotAcceptable);
            
            return _cache.GetString(key);
        }

        public async Task<string> GetStringAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new APIException(
                    "key is null...", HttpStatusCode.NotAcceptable);

            return await _cache.GetStringAsync(key);
        }

        public async Task<T> GetObject<T>(string key) where T : class
        {
            if (string.IsNullOrEmpty(key))
                throw new APIException(
                    "key is null...", HttpStatusCode.NotAcceptable);
            
            return HandleDeserialize<T>(await _cache.GetStringAsync(key));
        }

        private static T HandleDeserialize<T>(string value) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception e)
            {
                throw new APIException(
                    "can't serialize this object..", HttpStatusCode.NotAcceptable);
            }
        }
    }
}