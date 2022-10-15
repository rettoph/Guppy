using Guppy;
using Guppy.Common;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFilter<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, bool> filter, 
            int order)
                where TService : class
                where TImplementation : TService
        {
            var instance = new RuntimeServiceTypeFilter<TService, TImplementation>(filter, order);
            return services.AddSingleton<IServiceTypeFilter<TService>>(instance);
        }
    }
}
