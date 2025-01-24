using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Builder;
using Guppy.Core.Common.Configurations;

namespace Guppy.Core.Common.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterFilter(this ContainerBuilder builder, IServiceFilter filter)
        {
            builder.RegisterInstance(filter).As<IServiceFilter>().SingleInstance();

            return builder;
        }

        public static ContainerBuilder Configure<T>(this ContainerBuilder builder, Action<ILifetimeScope, T> configurator)
        {
            builder.RegisterInstance(new Configurator<T>(configurator)).As<Configurator>();

            return builder;
        }

        public static ContainerBuilder Configure<T>(this ContainerBuilder builder, Action<T> configurator)
        {
            builder.RegisterInstance(new Configurator<T>((_, instance) => configurator(instance))).As<Configurator>();

            return builder;
        }

        private static readonly ConditionalWeakTable<ContainerBuilder, HashSet<string>> _tags = [];
        private static HashSet<string> GetTags(ContainerBuilder builder)
        {
            if (_tags.TryGetValue(builder, out var tags))
            {
                return tags;
            }

            tags = [];
            _tags.Add(builder, tags);

            return tags;
        }

        public static ContainerBuilder AddTag(this ContainerBuilder builder, string tag)
        {
            GetTags(builder).Add(tag);

            return builder;
        }

        public static bool HasTag(this ContainerBuilder builder, string tag)
        {
            return GetTags(builder).Contains(tag);
        }

        public static ContainerBuilder EnsureRegisteredOnce(this ContainerBuilder builder, string tag, Action<ContainerBuilder> build)
        {
            if (builder.HasTag(tag))
            {
                return builder;
            }

            build(builder);

            builder.AddTag(tag);

            return builder;
        }

        public static IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> RegisterInstanceFrom<T>(this ContainerBuilder builder, ILifetimeScope scope)
            where T : class
        {
            return builder.RegisterInstance<T>(scope.Resolve<T>());
        }
    }
}