using Autofac;
using Guppy.Engine.Common;
using Guppy.Engine.Loaders;
using System.Runtime.CompilerServices;

namespace Guppy.Engine.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
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

        public static ContainerBuilder RegisterServiceLoader(this ContainerBuilder builder, Type type)
        {
            ThrowIf.Type.IsNotAssignableFrom<IServiceLoader>(type);

            builder.RegisterType(type).AsImplementedInterfaces().InstancePerLifetimeScope();

            return builder;
        }

        public static ContainerBuilder RegisterServiceLoader<T>(this ContainerBuilder builder)
            where T : IServiceLoader
        {
            builder.RegisterType<T>().AsImplementedInterfaces().InstancePerLifetimeScope();

            return builder;
        }
    }
}
