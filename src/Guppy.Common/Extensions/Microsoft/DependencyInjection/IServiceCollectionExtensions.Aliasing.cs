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
        public static IServiceCollection AddFilter(
            this IServiceCollection services,
            IFilter filter)
        {
            services.AddSingleton(filter);

            return services;
        }

        public static IServiceCollection AddFilters(
            this IServiceCollection services,
            IEnumerable<IFilter> filters)
        {
            foreach (var filter in filters)
            {
                services.AddFilter(filter);
            }

            return services;
        }

        public static IServiceCollection AddFilters(
            this IServiceCollection services,
            params IFilter[] filters)
        {
            foreach (var filter in filters)
            {
                services.AddFilter(filter);
            }

            return services;
        }
    }
}
