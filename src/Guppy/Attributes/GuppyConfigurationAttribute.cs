using Autofac;

namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public abstract class GuppyConfigurationAttribute : Attribute
    {
        public GuppyConfigurationAttribute()
        {
        }

        public virtual bool TryConfigure(ContainerBuilder builder, Type classType)
        {
            if (this.ShouldConfigure(builder, classType))
            {
                this.Configure(builder, classType);
                return true;
            }

            return false;
        }

        protected virtual bool ShouldConfigure(ContainerBuilder builder, Type classType)
        {
            return true;
        }

        protected abstract void Configure(ContainerBuilder builder, Type classType);
    }
}
