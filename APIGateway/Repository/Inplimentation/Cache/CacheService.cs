using EasyCaching.Core;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway.Repository.Inplimentation.Cache
{
    public interface IResponseCacheService
    {
        Task CatcheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCacheResponseAsync(string cacheKey);
        Task ResetCacheAsync(string cachekey);
    }
    public class ResponseCacheService : IResponseCacheService
    { 
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly IEasyCachingProviderFactory _providerFactory;
        public ResponseCacheService(IEasyCachingProviderFactory providerFactory)
        {
            this._providerFactory = providerFactory;
            this._cachingProvider = this._providerFactory.GetCachingProvider("redis1"); 
        }
        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            try
            {
                var cachedResponse = await _cachingProvider.GetAsync<string>(cacheKey);
                if (cachedResponse.HasValue)
                {
                    return cachedResponse.Value;
                }
                if (cachedResponse.IsNull)
                {
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public async Task ResetCacheAsync(string cachekey)
        { 
            await _cachingProvider.RemoveByPrefixAsync(cachekey);
        }
        public async Task CatcheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            try
            {
                if (response == null)
                {
                    return;
                }
                var serializeResponse = JsonConvert.SerializeObject(response);
                await _cachingProvider.SetAsync(cacheKey, serializeResponse, timeToLive);
            }
            catch (Exception) { }
            
        }
    }
}
