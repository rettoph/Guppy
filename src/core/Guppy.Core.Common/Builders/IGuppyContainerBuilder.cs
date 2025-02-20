using Autofac;

namespace Guppy.Core.Common.Builders
{
    public interface IGuppyContainerBuilder
    {
        ContainerBuilder ContainerBuilder { get; }
    }

    public interface IGuppyContainerBuilder<TSelf> : IGuppyContainerBuilder
        where TSelf : IGuppyContainerBuilder<TSelf>
    {
        TSelf Filter(Action<IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder>> filter, Action<IGuppyScopeBuilder> build);
    }
}
