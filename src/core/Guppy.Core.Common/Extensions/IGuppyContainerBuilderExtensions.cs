using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Features.OpenGenerics;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Implementations;
using Guppy.Core.Common.Systems;

namespace Guppy.Core.Common.Extensions
{
    public static class IGuppyContainerBuilderExtensions
    {
        public static TBuilder RegisterScopedSystem<TBuilder, TSystem>(this TBuilder builder)
            where TBuilder : IGuppyContainerBuilder
            where TSystem : IScopedSystem
        {
            builder.RegisterType<TSystem>().As<IScopedSystem>().InstancePerLifetimeScope();

            return builder;
        }

        public static TBuilder Configure<TBuilder, TConfig>(
            this TBuilder builder,
            Action<IGuppyScope, TConfig> configurator)
                where TBuilder : IGuppyContainerBuilder
        {
            builder.RegisterInstance(new Configurator<TConfig>(configurator)).As<Configurator>();

            return builder;
        }

        public static TBuilder Configure<TBuilder, TConfig>(
            this TBuilder builder,
            Action<TConfig> configurator)
                where TBuilder : IGuppyContainerBuilder
        {
            builder.RegisterInstance(new Configurator<TConfig>((_, instance) => configurator(instance))).As<Configurator>();

            return builder;
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterInstanceFrom<T>(this ContainerBuilder builder, ILifetimeScope scope)
            where T : class
        {
            return builder.RegisterInstance(scope.Resolve<T>());
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterInstance<T>(
            this IGuppyContainerBuilder builder,
            T instance)
                where T : class
        {
            return builder.ContainerBuilder.RegisterInstance(instance);
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(
            this IGuppyContainerBuilder builder,
            Func<IComponentContext, T> factory)
                where T : class
        {
            return builder.ContainerBuilder.Register(factory);
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> Register<T>(
            this IGuppyContainerBuilder builder,
            Func<ILifetimeScope, T> factory)
                where T : class
        {
            return builder.ContainerBuilder.Register(factory);
        }

        public static IRegistrationBuilder<T, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType<T>(
            this IGuppyContainerBuilder builder)
                where T : notnull
        {
            return builder.ContainerBuilder.RegisterType<T>();
        }

        public static IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> RegisterType(
            this IGuppyContainerBuilder builder,
            Type implementationType)
        {
            return builder.ContainerBuilder.RegisterType(implementationType);
        }

        public static IRegistrationBuilder<object, ReflectionActivatorData, DynamicRegistrationStyle> RegisterGeneric(
            this IGuppyContainerBuilder builder,
            Type implementor)
        {
            return builder.ContainerBuilder.RegisterGeneric(implementor);
        }

        public static IRegistrationBuilder<object, OpenGenericDelegateActivatorData, DynamicRegistrationStyle> RegisterGeneric(
            this IGuppyContainerBuilder builder,
            Func<IComponentContext, Type[], object> factory)
        {
            return builder.ContainerBuilder.RegisterGeneric(factory);
        }

        public static IModuleRegistrar RegisterModule<T>(
            this IGuppyContainerBuilder builder)
                where T : IModule, new()
        {
            return builder.ContainerBuilder.RegisterModule<T>();
        }

        private static readonly ConditionalWeakTable<IGuppyContainerBuilder, HashSet<string>> _tags = [];
        private static HashSet<string> GetTags(IGuppyContainerBuilder builder)
        {
            if (_tags.TryGetValue(builder, out var tags))
            {
                return tags;
            }

            tags = [];
            _tags.Add(builder, tags);

            return tags;
        }

        public static IGuppyContainerBuilder AddTag(this IGuppyContainerBuilder builder, string tag)
        {
            GetTags(builder).Add(tag);

            return builder;
        }

        public static bool HasTag(this IGuppyContainerBuilder builder, string tag)
        {
            return GetTags(builder).Contains(tag);
        }

        public static TBuilder EnsureRegisteredOnce<TBuilder>(this TBuilder builder, string tag, Action<TBuilder> build)
            where TBuilder : IGuppyContainerBuilder
        {
            if (builder.HasTag(tag))
            {
                return builder;
            }

            build(builder);

            builder.AddTag(tag);

            return builder;
        }
    }
}
