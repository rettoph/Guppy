using Guppy.Core.Common.Builders;

namespace Guppy.Core.Common
{
    public interface IGuppyContainerBuilderFilter<TBuilder>
        where TBuilder : IGuppyContainerBuilder
    {
        bool Filter(TBuilder builder);
        void Build(TBuilder builder);
    }
}
