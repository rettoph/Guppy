using Guppy.ECS.Definitions;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="definitionType"><see cref="ISystemDefinition"/> type.</param>
        /// <returns></returns>
        public static IServiceCollection AddSystem(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(ISystemDefinition), definitionType);
        }

        public static IServiceCollection AddSystem<TDefinition>(this IServiceCollection services)
            where TDefinition : class, ISystemDefinition
        {
            return services.AddSingleton<ISystemDefinition, TDefinition>();
        }

        public static IServiceCollection AddSystem(this IServiceCollection services, Type type, Func<IServiceProvider, bool> filter, int order)
        {
            return services.AddSingleton<ISystemDefinition>(new RuntimeSystemDefinition(type, filter, order));
        }

        public static IServiceCollection AddSystem<TSystem>(this IServiceCollection services, Func<IServiceProvider, bool> filter, int order)
            where TSystem : class, ISystem
        {
            return services.AddSingleton<ISystemDefinition>(new RuntimeSystemDefinition(typeof(TSystem), filter, order));
        }
    }
}
