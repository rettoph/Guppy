using Guppy;
using Guppy.Common;
using Guppy.MonoGame.Definitions;
using Guppy.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddGameComponent<T>(this IServiceCollection services)
            where T : class, IGameComponent
        {
            return services.AddScoped<T>()
                .AddAlias(Alias.Create<IGameComponent, T>());
        }

        public static IServiceCollection AddGameComponent(this IServiceCollection services, Type componentType)
        {
            return services.AddScoped(componentType)
                .AddAlias(new Alias(typeof(IGameComponent), componentType));
        }
    }
}
