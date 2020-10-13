using GOSLibraries.GOS_API_Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway
{
    public static class Cache
    {
        public static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(a => a.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
      
    }

    public class SessionTimer
    {
        public string UserId { get; set; } 
        public DateTime ClearAt { get; set; }
    }
    public class FailedCount
    {
        public int Counter { get; set; } 
        public TimeSpan RetryTime { get; set; }
    }

    //public interface IUriService
    //{
    //    Uri ReturnCompanyBaseDomain(string clientPath);
    //}
    //public class UriService : IUriService
    //{
    //    private readonly string _baseUri;
    //    public UriService(string baseUri)
    //    {
    //        _baseUri = baseUri;
    //    }
    //    public Uri ReturnCompanyBaseDomain(string clientPath)
    //    {
    //        return new Uri(_baseUri + clientPath);
    //    }
    //}
}
