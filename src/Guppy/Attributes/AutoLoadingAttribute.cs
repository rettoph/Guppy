using System.Reflection;

namespace Guppy.Attributes
{
    public abstract class AutoLoadingAttribute : InitializableAttribute
    {
        protected override bool ShouldInitialize(GuppyEngine engine, Type classType)
        {
            return classType.HasCustomAttribute<AutoLoadAttribute>() && base.ShouldInitialize(engine, classType);
        }
    }
}
