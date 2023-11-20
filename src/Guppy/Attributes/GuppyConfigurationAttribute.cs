using Autofac;
using Guppy.Configurations;

namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = true, Inherited = true)]
    public abstract class GuppyConfigurationAttribute : Attribute
    {
        public GuppyConfigurationAttribute()
        {
        }

        public virtual bool TryConfigure(GuppyConfiguration configuration, Type classType)
        {
            if(this.ShouldConfigure(configuration, classType))
            {
                this.Configure(configuration, classType);
                return true;
            }

            return false;
        }

        protected virtual bool ShouldConfigure(GuppyConfiguration configuration, Type classType)
        {
            return true;
        }

        protected abstract void Configure(GuppyConfiguration configuration, Type classType);
    }
}
