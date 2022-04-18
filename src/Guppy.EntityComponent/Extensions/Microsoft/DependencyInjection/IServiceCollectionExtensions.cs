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
        public static IServiceCollection AddSetup<TDefinition>(this IServiceCollection services)
            where TDefinition : SetupDefinition
        {
            return services.AddSingleton<SetupDefinition, TDefinition>();
        }

        public static IServiceCollection AddSetup(this IServiceCollection services, SetupDefinition definition)
        {
            return services.AddSingleton<SetupDefinition>(definition);
        }

        public static IServiceCollection AddSetup<TEntity>(this IServiceCollection services, Func<IServiceProvider, TEntity, bool> create, Func<IServiceProvider, TEntity, bool> destroy, int order)
            where TEntity : class, IEntity
        {
            return services.AddSetup(new RuntimeSetupDefinition<TEntity>(create, destroy, order));
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

        public static IServiceCollection AddComponen<TDefinition>(this IServiceCollection services)
            where TDefinition : ComponentDefinition
        {
            return services.AddSingleton<ComponentDefinition, TDefinition>();
        }

        public static IServiceCollection AddComponent(this IServiceCollection services, ComponentDefinition definition)
        {
            return services.AddSingleton<ComponentDefinition>(definition);
        }
        public static IServiceCollection AddComponent<TEntity, TComponent>(this IServiceCollection services, Func<IServiceProvider, TEntity, TComponent> factory)
            where TEntity : class, IEntity
            where TComponent : class, IComponent
        {
            return services.AddComponent(new RuntimeComponentDefinition(
                typeof(TEntity),
                typeof(TComponent),
                (p, e) => factory(p, (TEntity)e)));
        }

        public static IServiceCollection AddComponent<TEntity, TComponent>(this IServiceCollection services, Func<IServiceProvider, IEntity, TComponent> factory)
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
                        ComponentDefinition.DynamicFactory<TEntity, TComponent>()));
                }
    }
}
