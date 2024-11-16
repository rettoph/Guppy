using Autofac;
using Autofac.Core;
using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Extensions.Autofac;

namespace Guppy.Engine.Common.Attributes
{
    public sealed class RegisterModuleAttribute : GuppyConfigurationAttribute
    {
        protected override bool ShouldConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            return base.ShouldConfigure(boot, builder, classType) && classType.IsAssignableTo<IModule>() && classType.IsInterface == false && classType.IsAbstract == false;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterModule(classType, boot);
        }
    }
}
