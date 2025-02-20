using Guppy.Core.Common.Builders;

namespace Guppy.Core.Common.Delegates
{
    public delegate bool GuppyContainerFilterDelegate<TBuilder>(TBuilder builder)
        where TBuilder : IGuppyContainerBuilder;
}
