using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Extensions.Autofac;
using Guppy.Engine.Common.Loaders;

namespace Guppy.Engine.Common.Attributes
{
    internal sealed class ServiceLoaderAutoLoadAttribute : GuppyConfigurationAttribute
    {
        protected override bool ShouldConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            return base.ShouldConfigure(boot, builder, classType) && classType.IsAssignableTo<IServiceLoader>() && classType.IsInterface == false && classType.IsAbstract == false;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterServiceLoader(classType, boot);
        }
    }
}
