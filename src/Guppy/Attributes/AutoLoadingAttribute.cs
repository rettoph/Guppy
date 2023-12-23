using Autofac;
using System.Reflection;

namespace Guppy.Attributes
{
    public abstract class AutoLoadingAttribute : GuppyConfigurationAttribute
    {
        protected override bool ShouldConfigure(ContainerBuilder builder, Type classType)
        {
            bool result = classType.HasCustomAttribute<AutoLoadAttribute>();
            result &= base.ShouldConfigure(builder, classType);
            return result;
        }
    }
}
