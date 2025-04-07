using Guppy.Core.Common.Builders;

namespace Guppy.Core.Common.Extensions
{
    public static class IGuppyContainerBuilderFilterBuilderExtensions
    {
        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireScopeVariable<TVariable>(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter, TVariable required)
            where TVariable : IScopeVariable
        {
            return filter.Require(builder =>
            {
                if (builder.Variables.TryGet<TVariable>(out TVariable? variable) == false)
                {
                    return false;
                }

                return variable.Matches(required);
            });
        }

        public static IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> RequireEnvironmentVariable<TVariable>(
            this IGuppyContainerBuilderFilterBuilder<IGuppyScopeBuilder> filter, TVariable required)
            where TVariable : IEnvironmentVariable
        {
            return filter.Require(builder =>
            {
                if (builder.Root.EnvironmentVariables.TryGet<TVariable>(out TVariable? variable) == false)
                {
                    return false;
                }

                return variable.Matches(required);
            });
        }

        public static IGuppyContainerBuilderFilterBuilder<IGuppyRootBuilder> RequireEnvironmentVariable<TVariable>(
            this IGuppyContainerBuilderFilterBuilder<IGuppyRootBuilder> filter, TVariable required)
            where TVariable : IEnvironmentVariable
        {
            return filter.Require(builder =>
            {
                if (builder.EnvironmentVariables.TryGet<TVariable>(out TVariable? variable) == false)
                {
                    return false;
                }

                return variable.Matches(required);
            });
        }
    }
}
