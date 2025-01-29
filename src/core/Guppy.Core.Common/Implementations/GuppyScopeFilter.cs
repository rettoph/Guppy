using Guppy.Core.Common.Delegates;

namespace Guppy.Core.Common.Implementations
{
    public interface IGuppyScopeFilter
    {
        bool Filter(IGuppyScopeBuilder builder);
        void Build(IGuppyScopeBuilder builder);
    }

    public class GuppyScopeFilter(GuppyScopeFilterDelegate? filter, Action<IGuppyScopeBuilder> build) : IGuppyScopeFilter
    {
        private readonly GuppyScopeFilterDelegate? _filter = filter;
        private readonly Action<IGuppyScopeBuilder> _build = build;

        public void Build(IGuppyScopeBuilder builder)
        {
            this._build(builder);
        }

        public bool Filter(IGuppyScopeBuilder builder)
        {
            if (this._filter is null)
            {
                return true;
            }

            bool result = true;
            foreach (GuppyScopeFilterDelegate del in this._filter.GetInvocationList())
            {
                result &= del.Invoke(builder);
            }

            return result;
        }
    }
}
