using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Serialization;
using System;

namespace DynamicContract.Json
{
    /// <summary>
    /// The dynamic resolver for Newtonsoft.Json.Serialization.JsonContract for a given System.Type.
    /// </summary>
    public class DynamicContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// MemoryCache for JsonContract creation
        /// </summary>
        public IMemoryCache MemoryCache { get; set; }

        public override JsonContract ResolveContract(Type type)
        {
            if (MemoryCache == null)
            {
                throw new NullReferenceException($"{nameof(MemoryCache)} is missing, please add 'app.UseDynamicContractResolver();' in Startup.cs.");
            }

            if (NamingStrategy == null || NamingStrategy is not DynamicNamingStrategy namingStrategy)
            {
                throw new NullReferenceException($"{nameof(NamingStrategy)} is missing, please add 'app.UseDynamicContractResolver();' in Startup.cs.");
            }

            // make key for caching the JsonContract
            string key = namingStrategy.GetName() + type.FullName;

            // Get or create JsonContract
            JsonContract contract = MemoryCache.GetOrCreate(key, (entry) =>
            {
                entry.AbsoluteExpiration = DateTimeOffset.MaxValue;

                // create JsonContract
                return CreateContract(type);
            });

            return contract;
        }
    }
}
