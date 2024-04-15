using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Extensions.Autofac;

namespace Guppy.Engine.Common.Attributes
{
    internal sealed class ServiceLoaderAutoLoadAttribute : GuppyConfigurationAttribute
    {
        protected override bool ShouldConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            return base.ShouldConfigure(boot, builder, classType) && classType.IsInterface == false && classType.IsAbstract == false;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterServiceLoader(classType, boot);
        }
    }
}
