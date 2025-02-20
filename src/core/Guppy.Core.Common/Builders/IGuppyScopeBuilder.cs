namespace Guppy.Core.Common.Builders
{
    public interface IGuppyScopeBuilder : IGuppyContainerBuilder<IGuppyScopeBuilder>
    {
        IGuppyRoot Root { get; }
        IScopeVariableServiceBuilder Variables { get; }
    }
}
