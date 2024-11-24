using Autofac;
using Guppy.Core.Common.Extensions.System.Reflection;
using System.Reflection;

namespace Guppy.Core.Common.Attributes
{
    /// <summary>
    /// Custom attribute definind a configuration method. All defined attributes
    /// implementing this type will automatically be configured on engine boot
    /// </summary>
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
            foreach (Type type in assembly.GetTypes())
            {
                foreach (GuppyConfigurationAttribute attribute in type.GetAllCustomAttributes<GuppyConfigurationAttribute>(true))
                {
                    attribute.TryConfigure(boot, builder, type);
                }
            }
        }
    }
}
