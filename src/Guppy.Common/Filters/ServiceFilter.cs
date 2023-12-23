using Guppy.Common.Providers;

namespace Guppy.Common.Filters
{
    public class ServiceFilter : IServiceFilter
    {
        public readonly Type Type;

        public readonly object State;

        public ServiceFilter(Type type, object state)
        {
            this.State = state;
            this.Type = type;
        }

        public virtual bool AppliesTo(Type type)
        {
            if (this.Type.IsGenericTypeDefinition && type.ImplementsGenericTypeDefinition(this.Type))
            {
                return true;
            }

            var result = this.Type.IsAssignableFrom(type);

            return result;
        }

        public virtual bool Invoke(IStateProvider state)
        {
            bool result = state.Matches(this.State);
            return result;
        }
    }

    public class ServiceFilter<TService> : ServiceFilter
    {
        public ServiceFilter(object value) : base(typeof(TService), value)
        {
        }
    }
}
