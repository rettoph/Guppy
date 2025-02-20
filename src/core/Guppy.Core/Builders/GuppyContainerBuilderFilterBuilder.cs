using Guppy.Core.Common.Builders;
using Guppy.Core.Common.Delegates;

namespace Guppy.Core.Builders
{
    public class GuppyContainerBuilderFilterBuilder<TBuilder> : IGuppyContainerBuilderFilterBuilder<TBuilder>
        where TBuilder : IGuppyContainerBuilder
    {
        private GuppyContainerFilterDelegate<TBuilder>? _delegate;

        public IGuppyContainerBuilderFilterBuilder<TBuilder> Require(GuppyContainerFilterDelegate<TBuilder> filter)
        {
            this._delegate += filter;

            return this;
        }

        public GuppyContainerFilterDelegate<TBuilder>? Build()
        {
            return this._delegate;
        }
    }
}
