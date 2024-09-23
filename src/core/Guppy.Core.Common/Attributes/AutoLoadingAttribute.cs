using Autofac;
using Guppy.Core.Common.Extensions.System.Reflection;

namespace Guppy.Core.Common.Attributes
{
    public abstract class AutoLoadingAttribute : GuppyConfigurationAttribute
    {
        protected override bool ShouldConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            bool result = classType.HasCustomAttribute<AutoLoadAttribute>();
            result &= base.ShouldConfigure(boot, builder, classType);
            return result;
        }
    }
}
