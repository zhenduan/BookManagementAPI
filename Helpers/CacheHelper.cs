using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookManagementAPI.Helpers
{
    public static class CacheHelper
    {
        public static string GenerateCacheKey(QueryObject queryObject)
        {
            var serializedQuery = JsonSerializer.Serialize(queryObject);
            return $"Books_{serializedQuery}";
        }


    }
}