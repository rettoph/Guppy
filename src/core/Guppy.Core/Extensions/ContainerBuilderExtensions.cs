using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Contexts;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.Common.Services;
using Guppy.Core.Files.Extensions;
using Guppy.Core.Messaging;
using Guppy.Core.Serialization.Extensions;
using Guppy.Core.Services;

namespace Guppy.Core.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreServices(this ContainerBuilder builder, IGuppyContext context, IAssemblyService? assemblies = null)
        {
            if (builder.HasTag(nameof(RegisterCoreServices)))
            {
                return builder;
            }


            if (assemblies is null)
            {
                builder.RegisterType<AssemblyService>().As<IAssemblyService>().SingleInstance();
            }
            else
            {
                builder.RegisterInstance(assemblies).As<IAssemblyService>().SingleInstance();
            }

            builder.RegisterInstance(context).As<IGuppyContext>().SingleInstance();

            builder.RegisterType<Tags>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<Broker<IMessage>>().As<IBroker<IMessage>>().InstancePerDependency();

            builder.RegisterGeneric(typeof(Lazier<>)).As(typeof(Lazier<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(Scoped<>)).As(typeof(IScoped<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(Filtered<>)).As(typeof(IFiltered<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(Configuration<>)).As(typeof(IConfiguration<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(Optional<>)).As(typeof(IOptional<>)).InstancePerDependency();

            builder.RegisterType<FilteredService>().As<IFilteredService>().InstancePerLifetimeScope();
            builder.RegisterType<ServiceFilterService>().As<IServiceFilterService>().InstancePerLifetimeScope();

            builder.RegisterCoreSerializationServices().RegisterCoreFileServices();

            return builder.AddTag(nameof(RegisterCoreServices));
        }
    }
}
