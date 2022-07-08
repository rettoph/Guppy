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
        public static IServiceCollection AddResource<T>(this IServiceCollection services)
            where T : class, IResourceDefinition
        {
            return services.AddSingleton<IResourceDefinition, T>();
        }

        public static IServiceCollection AddResource(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(IResourceDefinition), definitionType);
        }

        public static IServiceCollection AddResource<T>(this IServiceCollection services, string name, string? source)
        {
            return services.AddSingleton<IResourceDefinition>(new RuntimeResourceDefinition<T>(name, source));
        }

        public static IServiceCollection AddStringResource(this IServiceCollection services, string name)
        {
            return services.AddResource<string>(name, null);
        }
    }
}
