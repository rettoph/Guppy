namespace Guppy.Core.Common.Builders
{
    public interface IGuppyRootBuilder : IGuppyContainerBuilder<IGuppyRootBuilder>
    {
        IEnvironmentVariableServiceBuilder EnvironmentVariables { get; }

        IGuppyRootBuilder Filter(Action<IGuppyContainerBuilderFilterBuilder<IGuppyRootBuilder>> filter, Action<IGuppyRootBuilder> build);
    }
}
