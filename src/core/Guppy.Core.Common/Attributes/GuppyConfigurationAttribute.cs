using Autofac;
using System.Reflection;

namespace Guppy.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public abstract class GuppyConfigurationAttribute : Attribute
    {
        public GuppyConfigurationAttribute()
        {
        }

        public virtual bool TryConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            if (this.ShouldConfigure(boot, builder, classType))
            {
                this.Configure(boot, builder, classType);
                return true;
            }

            return false;
        }

        protected virtual bool ShouldConfigure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            return true;
        }

        protected abstract void Configure(IContainer boot, ContainerBuilder builder, Type classType);

        public static void TryConfigureAllInAssembly(Assembly assembly, IContainer boot, ContainerBuilder builder)
        {
            foreach (Type type in assembly.GetTypes().Where(x => x.HasCustomAttributesIncludingInterfaces<GuppyConfigurationAttribute>(true)))
            {
                foreach (GuppyConfigurationAttribute attribute in type.GetCustomAttributesIncludingInterfaces<GuppyConfigurationAttribute>(true))
                {
                    attribute.TryConfigure(boot, builder, type);
                }
            }
        }
    }
}
