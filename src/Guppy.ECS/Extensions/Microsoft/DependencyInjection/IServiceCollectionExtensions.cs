using Guppy.Attributes;
using Guppy.Common;
using Guppy.ECS;
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
        public static IServiceCollection ConfigureEntity(this IServiceCollection services, EntityKey key, params EntityTag[] tags)
        {
            return services.AddSingleton<IEntityTypeDefinition>(new EntityTypeDefinition(key, tags));
        }

        public static IServiceCollection AddComponent<T>(this IServiceCollection services, Func<IServiceProvider, T> factory, params EntityTag[] tags)
            where T : class
        {
            return services.AddScoped<IComponentDefinition>(p => new RuntimeComponentDefinition<T>(p, factory, tags));
        }

        public static IServiceCollection AddComponent(this IServiceCollection services, Type definitionType)
        {
            return services.AddScoped(typeof(IComponentDefinition), definitionType);
        }

        public static IServiceCollection AddComponent<TDefinition>(this IServiceCollection services)
            where TDefinition : class, IComponentDefinition
        {
            return services.AddScoped<IComponentDefinition, TDefinition>();
        }

        public static IServiceCollection AddSystem(this IServiceCollection services, Type definitionType)
        {
            return services.AddSingleton(typeof(ISystemDefinition), definitionType);
        }

        public static IServiceCollection AddSystem<TDefinition>(this IServiceCollection services)
            where TDefinition : class, ISystemDefinition
        {
            return services.AddSingleton<ISystemDefinition, TDefinition>();
        }

        public static IServiceCollection AddSystem(this IServiceCollection services, Type type, int order, params IFilter<ISystemDefinition>[] filters)
        {
            var definition = new RuntimeSystemDefinition(type, order, filters);

            return services.AddSingleton<ISystemDefinition>(definition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSystem"></typeparam>
        /// <param name="services"></param>
        /// <param name="order"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IServiceCollection AddSystem<TSystem>(this IServiceCollection services, int order, params IFilter<ISystemDefinition>[] filters)
            where TSystem : class, ISystem
        {
            var definition = new RuntimeSystemDefinition(typeof(TSystem), order, filters);

            return services.AddSingleton<ISystemDefinition>(definition);
        }
    }
}
