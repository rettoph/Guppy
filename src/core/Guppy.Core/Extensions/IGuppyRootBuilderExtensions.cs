using Autofac;
using Guppy.Core.Common;
using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Extensions;
using Guppy.Core.Common.Services;
using Guppy.Core.Files.Extensions;
using Guppy.Core.Messaging.Extensions;
using Guppy.Core.Resources.Extensions;
using Guppy.Core.Serialization.Extensions;
using Guppy.Core.Services;

namespace Guppy.Core.Extensions
{
    public static class IGuppyRootBuilderExtensions
    {
        public static IGuppyRootBuilder RegisterCoreServices(this IGuppyRootBuilder builder, IAssemblyService? assemblies = null)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreServices), builder =>
            {
                if (assemblies is null)
                {
                    builder.Register<AssemblyService>(AssemblyService.Factory).As<IAssemblyService>().SingleInstance();
                }
                else
                {
                    builder.RegisterInstance(assemblies).As<IAssemblyService>().SingleInstance();
                }

                builder.RegisterType<GlobalSystemService>().As<IGlobalSystemService>().SingleInstance();
                builder.RegisterType<ScopedSystemService>().As<IScopedSystemService>().InstancePerLifetimeScope();

                builder.RegisterType<Tags>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
                builder.RegisterGeneric(typeof(Filtered<>)).As(typeof(IFiltered<>)).InstancePerDependency();
                builder.RegisterGeneric(typeof(Configuration<>)).As(typeof(IConfiguration<>)).InstancePerDependency();
                builder.RegisterGeneric(typeof(Optional<>)).As(typeof(IOptional<>)).InstancePerDependency();
                //builder.RegisterGeneric(typeof(Lazier<>)).As(typeof(Lazy<>)).InstancePerDependency();

                builder.RegisterType<ServiceFilterService>().As<IServiceFilterService>().InstancePerLifetimeScope();
                builder.RegisterType<ConfigurationService>().As<IConfigurationService>().InstancePerLifetimeScope();

                builder.RegisterCoreSerializationServices()
                    .RegisterCoreFileServices()
                    .RegisterCoreMessagingServices()
                    .RegisterCoreResourcesServices();
            });
        }
    }
}