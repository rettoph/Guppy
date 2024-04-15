using Autofac;
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
            where T : new()
        {
            services.RegisterInstance(new ConfigurationBuilder<T>(builder));
        }

        private static readonly ConditionalWeakTable<ContainerBuilder, HashSet<string>> _tags = new();
        private static HashSet<string> GetTags(ContainerBuilder builder)
        {
            if (_tags.TryGetValue(builder, out var tags))
            {
                return tags;
            }

            tags = new HashSet<string>();
            _tags.Add(builder, tags);

            return tags;
        }

        public static ContainerBuilder AddTag(this ContainerBuilder builder, string tag)
        {
            GetTags(builder).Add(tag);

            return builder;
        }

        public static bool HasTag(this ContainerBuilder engine, string tag)
        {
            return GetTags(engine).Contains(tag);
        }
    }
}
