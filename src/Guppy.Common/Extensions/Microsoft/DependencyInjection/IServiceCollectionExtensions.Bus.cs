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
        public static IServiceCollection AddBusConfiguration<T>(
            this IServiceCollection services,
            int queue)
                where T : notnull
        {
            return services.AddSingleton<BusConfiguration>(new BusConfiguration<T>(queue));
        }
    }
}
