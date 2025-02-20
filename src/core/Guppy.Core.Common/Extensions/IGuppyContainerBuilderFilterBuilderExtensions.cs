using Guppy.Core.Common.Builders;

namespace Guppy.Core.Common.Extensions
{
    public static class IGuppyContainerBuilderFilterBuilderExtensions
    {
        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireScopeVariable<TVariable>(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter, TVariable value)
            where TVariable : IScopeVariable
        {
            return filter.Require(builder => builder.Variables.Get<TVariable>()?.Matches(value) ?? false);
        }

        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireEnvironmentVariable<TVariable>(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter, TVariable value)
            where TVariable : IEnvironmentVariable
        {
            return filter.Require(builder => builder.Root.EnvironmentVariables.Matches(value));
        }

        public static IGuppyContainerBuilderFilterBuilder<IGuppyRootBuilder> RequireEnvironmentVariable<TVariable>(
            this IGuppyContainerBuilderFilterBuilder<IGuppyRootBuilder> filter, TVariable value)
            where TVariable : IEnvironmentVariable
        {
            return filter.Require(builder => builder.EnvironmentVariables.Get<TVariable>()?.Matches(value) ?? false);
        }
    }
}
