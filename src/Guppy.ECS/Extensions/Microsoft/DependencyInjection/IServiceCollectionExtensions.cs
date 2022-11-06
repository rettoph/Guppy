using Guppy.Attributes;
using Guppy.Common;
using Guppy.ECS;
using Microsoft.Extensions.DependencyInjection;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions_ECS
    {
        private static readonly MethodInfo AddComponentTypeMethodInfo = typeof(IServiceCollectionExtensions_ECS)
            .GetMethod(
                name: nameof(AddComponentType),
                bindingAttr: BindingFlags.Public | BindingFlags.Static,
                types: new[]
                {
                            typeof(IServiceCollection)
                }) ?? throw new NotImplementedException();

        public static IServiceCollection AddSystem<TSystem>(this IServiceCollection services)
            where TSystem : class, ISystem
        {
            return services.AddScoped<TSystem>().AddAlias(Alias.Create<ISystem, TSystem>());
        }

        public static IServiceCollection AddSystem(this IServiceCollection services, Type implementationType)
        {
            return services.AddScoped(implementationType).AddAlias(new Alias(typeof(ISystem), implementationType));
        }

        public static IServiceCollection AddComponentType(this IServiceCollection services, Type type)
        {
            var addComponentTypeMethod = AddComponentTypeMethodInfo.MakeGenericMethod(type);

            addComponentTypeMethod.Invoke(null, new object[] { services });

            return services;
        }

        public static IServiceCollection AddComponentType<T>(this IServiceCollection services)
            where T : class
        {
            services.AddSingleton<ComponentType>(ComponentType.Create<T>());

            return services;
        }
    }
}
