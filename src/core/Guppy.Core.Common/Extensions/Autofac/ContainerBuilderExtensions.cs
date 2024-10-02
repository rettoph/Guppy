using Autofac;
using Autofac.Builder;
using System.Runtime.CompilerServices;

namespace Guppy.Core.Common.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterFilter(this ContainerBuilder builder, IServiceFilter filter)
        {
            builder.RegisterInstance(filter).As<IServiceFilter>().SingleInstance();
        }

        public static void Configure<T>(this ContainerBuilder services, Action<ILifetimeScope, T> builder)
        {
            services.RegisterInstance(new ConfigurationBuilder<T>(builder)).As<ConfigurationBuilder>();
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
