using Autofac;
using Guppy.Common.Autofac;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisteGuppyCommon(this ContainerBuilder services)
        {
            services.RegisterGeneric(typeof(Lazier<>)).As(typeof(Lazier<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Scoped<>)).As(typeof(IScoped<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Filtered<>)).As(typeof(IFiltered<>)).InstancePerDependency();
            services.RegisterGeneric(typeof(Options<>)).As(typeof(IOptions<>)).InstancePerDependency();

            services.RegisterType<Broker>().As<IBroker>().InstancePerDependency();
            services.RegisterType<Bus>().As<IBus>().InstancePerMatchingLifetimeScope(LifetimeScopeTags.Guppy);
            services.RegisterType<ServiceFilterProvider>().As<IServiceFilterProvider>().InstancePerLifetimeScope();
            services.RegisterType<FilteredProvider>().As<IFilteredProvider>().InstancePerLifetimeScope();
            services.RegisterType<StateProvider>().As<IStateProvider>().InstancePerLifetimeScope();
        }

        public static void Configure<T>(this ContainerBuilder services, Action<T> builder)
            where T : new()
        {
            services.RegisterInstance(new OptionBuilder<T>(builder));
        }

        public static void AddFilter(this ContainerBuilder builder, IServiceFilter filter)
        {
            builder.RegisterInstance(filter).As<IServiceFilter>().SingleInstance();
        }
    }
}
