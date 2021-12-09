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
        private readonly DefaultNamingStrategy pascalCaseNamingStrategy;
        private readonly CamelCaseNamingStrategy camelCaseNamingStrategy;
        private readonly SnakeCaseNamingStrategy snakeCaseNamingStrategy;
        private readonly KebabCaseNamingStrategy kebabCaseNamingStrategy;


        public DynamicNamingStrategy(
            IHttpContextAccessor httpContextAccessor,
            DefaultNamingStrategy pascalCaseNamingStrategy,
            CamelCaseNamingStrategy camelCaseNamingStrategy,
            SnakeCaseNamingStrategy snakeCaseNamingStrategy,
            KebabCaseNamingStrategy kebabCaseNamingStrategy
            )
        {
            this.httpContextAccessor = httpContextAccessor;
            this.pascalCaseNamingStrategy = pascalCaseNamingStrategy;
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
                // default casing
                return "camelCase";
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

            if (IsPascalCase(namingStrategyHeader))
            {
                return pascalCaseNamingStrategy;
            }
            else if (IsSnakeCase(namingStrategyHeader))
            {
                return snakeCaseNamingStrategy;
            }
            else if (IsKebabCase(namingStrategyHeader))
            {
                return kebabCaseNamingStrategy;
            }

            // camel case is default for this system
            return camelCaseNamingStrategy;
        }

        protected override string ResolvePropertyName(string name)
        {
            NamingStrategy strategy = GetNamingStrategy();

            return strategy.GetPropertyName(name, false);
        }

        public override string GetPropertyName(string name, bool hasSpecifiedName)
        {
            // overwrite if the property has specified name.
            // Note: property name must be PascalCase
            return ResolvePropertyName(name);
        }

        #region Helper

        private bool IsPascalCase(string name) => "PascalCase".Equals(name, StringComparison.OrdinalIgnoreCase);
        private bool IsSnakeCase(string name) => "snake_case".Equals(name, StringComparison.OrdinalIgnoreCase);
        private bool IsKebabCase(string name) => "kebab-case".Equals(name, StringComparison.OrdinalIgnoreCase);

        #endregion Helper
    }
}
