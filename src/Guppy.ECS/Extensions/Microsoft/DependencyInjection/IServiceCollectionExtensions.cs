using Guppy.Attributes;
using Guppy.Common;
using Guppy.ECS;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSystem<TSystem>(this IServiceCollection services)
            where TSystem : class, ISystem
        {
            return services.AddScoped<TSystem>().AddAlias(Alias.Create<ISystem, TSystem>());
        }

        public static IServiceCollection AddSystem(this IServiceCollection services, Type implementationType)
        {
            return services.AddScoped(implementationType).AddAlias(new Alias(typeof(ISystem), implementationType));
        }
    }
}
