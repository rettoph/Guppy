using System.Runtime.CompilerServices;
using Autofac;
using Autofac.Builder;
using Guppy.Core.Common.Implementations;

namespace Guppy.Core.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        [Obsolete]
        public static IGuppyScopeBuilder RegisterFilter(this IGuppyScopeBuilder builder, IServiceFilter filter)
        {
            builder.RegisterInstance(filter).As<IServiceFilter>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterGlobalSystem<TSystem>(this IGuppyScopeBuilder builder)
            where TSystem : IGlobalSystem
        {
            builder.RegisterType<TSystem>().As<IGlobalSystem>().SingleInstance();

            return builder;
        }

        public static IGuppyScopeBuilder RegisterScopedSystem<TSystem>(this IGuppyScopeBuilder builder)
            where TSystem : IScopedSystem
        {
            builder.RegisterType<TSystem>().As<IGlobalSystem>().InstancePerLifetimeScope();

            return builder;
        }

        public static IGuppyScopeBuilder Configure<T>(this IGuppyScopeBuilder builder, Action<IGuppyScope, T> configurator)
        {
            builder.RegisterInstance(new Configurator<T>(configurator)).As<Configurator>();

            return builder;
        }

        public static IGuppyScopeBuilder Configure<T>(this IGuppyScopeBuilder builder, Action<T> configurator)
        {
            builder.RegisterInstance(new Configurator<T>((_, instance) => configurator(instance))).As<Configurator>();

            return builder;
        }

        private static readonly ConditionalWeakTable<IGuppyScopeBuilder, HashSet<string>> _tags = [];
        private static HashSet<string> GetTags(IGuppyScopeBuilder builder)
        {
            if (_tags.TryGetValue(builder, out var tags))
            {
                return tags;
            }

            tags = [];
            _tags.Add(builder, tags);

            return tags;
        }

        public static IGuppyScopeBuilder AddTag(this IGuppyScopeBuilder builder, string tag)
        {
            GetTags(builder).Add(tag);

            return builder;
        }

        public static bool HasTag(this IGuppyScopeBuilder builder, string tag)
        {
            return GetTags(builder).Contains(tag);
        }

        public static IGuppyScopeBuilder EnsureRegisteredOnce(this IGuppyScopeBuilder builder, string tag, Action<IGuppyScopeBuilder> build)
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
            return builder.RegisterInstance(scope.Resolve<T>());
        }
    }
}