using Autofac;
using Guppy.Common.Implementations;
using Guppy.Common.Providers;
using Guppy.Common.Services;

namespace Guppy.Common.Extensions.Autofac
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisteGuppyCommon(this ContainerBuilder services)
        {
            services.RegisterType<ServiceFilterProvider>().As<IServiceFilterProvider>().InstancePerLifetimeScope();
            services.RegisterType<BulkSubscriptionService>().As<IBulkSubscriptionService>().InstancePerLifetimeScope();
        }

        public static void RegisterFilter(this ContainerBuilder builder, IServiceFilter filter)
        {
            builder.RegisterInstance(filter).As<IServiceFilter>().SingleInstance();
        }
    }
}
