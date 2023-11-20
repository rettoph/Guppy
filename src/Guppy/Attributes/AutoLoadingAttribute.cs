using System.Reflection;
using Autofac;
using Guppy.Configurations;

namespace Guppy.Attributes
{
    public abstract class AutoLoadingAttribute : GuppyConfigurationAttribute
    {
        protected override bool ShouldConfigure(GuppyConfiguration configuration, Type classType)
        {
            bool result = classType.HasCustomAttribute<AutoLoadAttribute>();
            result &= base.ShouldConfigure(configuration, classType);
            return result;
        }
    }
}
