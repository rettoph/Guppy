using Autofac;
using Guppy.Core.Common.Extensions.System.Reflection;

namespace Guppy.Core.Common.Attributes
{
    /// <summary>
    /// Custom GuppyConfiguration attribute that will only be invoked
    /// if the defining type also contains an <see cref="AutoLoadAttribute"/>
    /// </summary>
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
