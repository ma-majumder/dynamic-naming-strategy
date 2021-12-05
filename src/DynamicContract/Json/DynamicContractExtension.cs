using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System;

namespace DynamicContract.Json
{
    public static class DynamicContractExtension
    {
        /// <summary>
        /// Configures Newtonsoft.Json specific dynamic contract resolver.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcBuilder.</param>
        /// <param name="setupAction">Callback to configure Microsoft.AspNetCore.Mvc.MvcNewtonsoftJsonOptions.</param>
        /// <returns></returns>
        public static IMvcBuilder AddDynamicContractResolver(this IMvcBuilder builder, Action<MvcNewtonsoftJsonOptions> setupAction = null)
        {
            var services = builder.Services;

            services.AddSingleton<CamelCaseNamingStrategy>();
            services.AddSingleton<SnakeCaseNamingStrategy>();
            services.AddSingleton<KebabCaseNamingStrategy>();

            services.AddSingleton<DynamicNamingStrategy>();

            DynamicContractResolver contractResolver = new DynamicContractResolver();
            services.AddSingleton(contractResolver);

            builder.AddNewtonsoftJson(options =>
            {
                setupAction?.Invoke(options);

                options.SerializerSettings.ContractResolver = contractResolver;
            });

            return builder;
        }

        /// <summary>
        /// Add dynamic naming strategy to dynamic contract resolver
        /// </summary>
        /// <param name="builder"></param>
        public static void UseDynamicContractResolver(this IApplicationBuilder builder)
        {
            DynamicContractResolver contractResolver = builder.ApplicationServices.GetRequiredService<DynamicContractResolver>();

            // set dynamic naming strategy
            contractResolver.NamingStrategy = builder.ApplicationServices.GetRequiredService<DynamicNamingStrategy>();

            // set memory cache
            contractResolver.MemoryCache = builder.ApplicationServices.GetRequiredService<IMemoryCache>();
        }
    }
}
