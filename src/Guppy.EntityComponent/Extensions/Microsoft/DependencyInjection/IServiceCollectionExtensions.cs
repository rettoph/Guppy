using Guppy.EntityComponent;
using Guppy.EntityComponent.Definitions;
using Guppy.EntityComponent.Definitions.ComponentFilters;
using Guppy.EntityComponent.Definitions.Components;
using Guppy.EntityComponent.Definitions.Setups;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSetup(this IServiceCollection services, Type setupDefinitionType)
        {
            return services.AddScoped(typeof(SetupDefinition), setupDefinitionType);
        }

        public static IServiceCollection AddSetup<TDefinition>(this IServiceCollection services)
            where TDefinition : SetupDefinition
        {
            return services.AddScoped<SetupDefinition, TDefinition>();
        }

        public static IServiceCollection AddSetup(this IServiceCollection services, SetupDefinition definition)
        {
            return services.AddScoped<SetupDefinition>(p => definition);
        }

        public static IServiceCollection AddSetup<TEntity>(this IServiceCollection services, Func<TEntity, bool> initialize, Func<TEntity, bool> uninitialize, int order)
            where TEntity : class, IEntity
        {
            return services.AddSetup(new RuntimeSetupDefinition<TEntity>(initialize, uninitialize, order));
        }

        public static IServiceCollection AddComponentFilter(this IServiceCollection services, Type componentFilterDefinitionType)
        {
            return services.AddSingleton(typeof(ComponentFilterDefinition), componentFilterDefinitionType);
        }

        public static IServiceCollection AddComponentFilter<TDefinition>(this IServiceCollection services)
            where TDefinition : ComponentFilterDefinition
        {
            return services.AddSingleton<ComponentFilterDefinition, TDefinition>();
        }

        public static IServiceCollection AddComponentFilter(this IServiceCollection services, ComponentFilterDefinition definition)
        {
            return services.AddSingleton<ComponentFilterDefinition>(definition);
        }
        public static IServiceCollection AddComponentFilter<TComponent>(this IServiceCollection services, Func<IServiceProvider, IEntity, ComponentDefinition, bool> entityFilter, Func<Type, ComponentDefinition, bool> typeFilter)
            where TComponent : class, IComponent
        {
            return services.AddComponentFilter(new RuntimeComponentFilterDefinition(typeof(TComponent), entityFilter, typeFilter));
        }

        public static IServiceCollection AddComponent(this IServiceCollection services, Type componentDefinitionType)
        {
            return services.AddScoped(typeof(ComponentDefinition), componentDefinitionType);
        }

        public static IServiceCollection AddComponen<TDefinition>(this IServiceCollection services)
            where TDefinition : ComponentDefinition
        {
            return services.AddScoped<ComponentDefinition, TDefinition>();
        }

        public static IServiceCollection AddComponent(this IServiceCollection services, ComponentDefinition definition)
        {
            return services.AddScoped<ComponentDefinition>(p => definition);
        }
        public static IServiceCollection AddComponent<TEntity, TComponent>(this IServiceCollection services, Func<IServiceProvider, TComponent> factory)
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            return services.AddComponent(new RuntimeComponentDefinition(
                typeof(TEntity),
                typeof(TComponent),
                factory));
        }
        public static IServiceCollection AddComponent<TEntity, TComponent>(this IServiceCollection services)
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            return services.AddComponent(new RuntimeComponentDefinition(
                typeof(TEntity),
                typeof(TComponent),
                ComponentDefinition.DynamicFactory<TComponent>()));
        }
    }
}
