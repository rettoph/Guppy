using System.Reflection;
using Guppy.Configurations;

namespace Guppy.Attributes
{
    public abstract class AutoLoadingAttribute : GuppyConfigurationAttribute
    {
        protected override bool ShouldConfigure(GuppyConfiguration configuration, Type classType)
        {
            return classType.HasCustomAttribute<AutoLoadAttribute>() && base.ShouldConfigure(configuration, classType);
        }
    }
}
