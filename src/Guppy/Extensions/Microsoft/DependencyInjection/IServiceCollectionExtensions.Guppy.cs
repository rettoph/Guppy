using Guppy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddGuppy<TGuppy>(this IServiceCollection services)
            where TGuppy : class, IGuppy
        {
            return services.AddFaceted<IGuppy, TGuppy>(ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddGuppy<TGuppy>(this IServiceCollection services, Func<IServiceProvider, TGuppy> factory)
            where TGuppy : class, IGuppy
        {
            return services.AddFaceted<IGuppy, TGuppy>(factory, ServiceLifetime.Scoped);
        }
    }
}
