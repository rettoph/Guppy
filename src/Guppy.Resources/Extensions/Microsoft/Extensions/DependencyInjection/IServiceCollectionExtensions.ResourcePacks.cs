using Guppy;
using Guppy.Providers;
using Guppy.Resources.Definitions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddResourcePack<T>(this IServiceCollection services)
            where T : class, IResourcePackDefinition
        {
            return services.AddSingleton<IResourcePackDefinition, T>();
        }

        public static IServiceCollection AddResourcePack(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(IResourcePackDefinition), definitionType);
        }

        public static IServiceCollection AddResourcePack(this IServiceCollection services, string name, string path)
        {
            return services.AddSingleton<IResourcePackDefinition>(new RuntimePackDefinition(name, path));
        }
    }
}
