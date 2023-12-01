using Autofac;
using Guppy.Common;
using Guppy.Common.Autofac;
using Guppy.Common.Extensions.Autofac;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using Guppy.Common.Services;
using Guppy.Loaders;
using Guppy.Messaging;
using Guppy.Providers;
using Guppy.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Extensions.Autofac
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
