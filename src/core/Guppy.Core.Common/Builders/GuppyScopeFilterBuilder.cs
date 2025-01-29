using Guppy.Core.Common.Delegates;

namespace Guppy.Core.Common.Builders
{
    public class GuppyScopeFilterBuilder
    {
        private GuppyScopeFilterDelegate? _delegate;

        public GuppyScopeFilterBuilder Require(GuppyScopeFilterDelegate filter)
        {
            this._delegate += filter;

            return this;
        }

        public GuppyScopeFilterDelegate? Build()
        {
            return this._delegate;
        }
    }
}
