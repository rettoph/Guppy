namespace Guppy.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
    public abstract class InitializableAttribute : Attribute
    {
        public InitializableAttribute()
        {
        }

        public virtual bool TryInitialize(GuppyEngine engine, Type classType)
        {
            if(this.ShouldInitialize(engine, classType))
            {
                this.Initialize(engine, classType);
                return true;
            }

            return false;
        }

        protected virtual bool ShouldInitialize(GuppyEngine engine, Type classType)
        {
            return true;
        }

        protected abstract void Initialize(GuppyEngine engine, Type classType);
    }
}
