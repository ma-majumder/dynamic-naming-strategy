using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace DynamicContract.Json
{
    /// <summary>
    /// This class will resolve how property names and dictionary keys are serialized based on request header.
    /// </summary>
    public class DynamicNamingStrategy : NamingStrategy
    {
        private const string headerName = "x-json-naming-strategy";

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CamelCaseNamingStrategy camelCaseNamingStrategy;
        private readonly SnakeCaseNamingStrategy snakeCaseNamingStrategy;
        private readonly KebabCaseNamingStrategy kebabCaseNamingStrategy;

        public DynamicNamingStrategy(
            IHttpContextAccessor httpContextAccessor,
            CamelCaseNamingStrategy camelCaseNamingStrategy, 
            SnakeCaseNamingStrategy snakeCaseNamingStrategy, 
            KebabCaseNamingStrategy kebabCaseNamingStrategy
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.camelCaseNamingStrategy = camelCaseNamingStrategy;
            this.snakeCaseNamingStrategy = snakeCaseNamingStrategy;
            this.kebabCaseNamingStrategy = kebabCaseNamingStrategy;
        }

        /// <summary>
        /// Get the name of requested NamingStrategy
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;

            // get naming strategy from request header
            string name = httpContext.Request.Headers[headerName].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(name))
            {
                return NamingStrategies.CamelCase;
            }

            return name;
        }

        /// <summary>
        /// Get requested NamingStrategy
        /// </summary>
        /// <returns></returns>
        public NamingStrategy GetNamingStrategy()
        {
            var namingStrategyHeader = GetName();

            if (NamingStrategies.IsSnakeCase(namingStrategyHeader))
            {
                return snakeCaseNamingStrategy;
            }
            else if (NamingStrategies.IsKebabCase(namingStrategyHeader))
            {
                return kebabCaseNamingStrategy;
            }

            return camelCaseNamingStrategy;
        }

        protected override string ResolvePropertyName(string name)
        {
            NamingStrategy strategy = GetNamingStrategy();

            return strategy.GetPropertyName(name, false);
        }
    }

    #region Helper

    class NamingStrategies
    {
        public const string CamelCase = "camelCase";
        public const string SnakeCase = "snake_case";
        public const string KebabCase = "kebab-case";

        public static bool IsCamelCase(string name) => CamelCase.Equals(name, StringComparison.OrdinalIgnoreCase);
        public static bool IsSnakeCase(string name) => SnakeCase.Equals(name, StringComparison.OrdinalIgnoreCase);
        public static bool IsKebabCase(string name) => KebabCase.Equals(name, StringComparison.OrdinalIgnoreCase);
    }

    #endregion Helper
}
