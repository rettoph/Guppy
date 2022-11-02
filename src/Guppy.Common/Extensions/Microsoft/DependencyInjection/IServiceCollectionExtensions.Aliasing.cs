using Guppy;
using Guppy.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAlias(
            this IServiceCollection services, 
            Alias alias)
        {
            return services.AddSingleton(alias);
        }

        public static IServiceCollection AddAliases(
            this IServiceCollection services, 
            IEnumerable<Alias> aliases)
        {
            foreach(var alias in aliases)
            {
                services.AddAlias(alias);
            }

            return services;
        }

        public static IServiceCollection AddAliases(
            this IServiceCollection services,
            params Alias[] aliases)
        {
            foreach (var alias in aliases)
            {
                services.AddAlias(alias);
            }

            return services;
        }

        public static IServiceCollection AddFilter(
            this IServiceCollection services,
            IServiceFilter filter)
        {
            services.AddSingleton(filter);

            return services;
        }

        public static IServiceCollection AddFilters(
            this IServiceCollection services,
            IEnumerable<IServiceFilter> filters)
        {
            foreach (var filter in filters)
            {
                services.AddFilter(filter);
            }

            return services;
        }

        public static IServiceCollection AddFilters(
            this IServiceCollection services,
            params IServiceFilter[] filters)
        {
            foreach (var filter in filters)
            {
                services.AddFilter(filter);
            }

            return services;
        }
    }
}
