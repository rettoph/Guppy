using Guppy.Core.Common.Delegates;

namespace Guppy.Core.Common.Builders
{
    public interface IGuppyContainerBuilderFilterBuilder<TBuilder>
        where TBuilder : IGuppyContainerBuilder
    {
        IGuppyContainerBuilderFilterBuilder<TBuilder> Require(GuppyContainerFilterDelegate<TBuilder> filter);

        GuppyContainerFilterDelegate<TBuilder>? Build();
    }
}
