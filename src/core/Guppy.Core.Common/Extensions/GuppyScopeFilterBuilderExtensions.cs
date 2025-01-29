using Guppy.Core.Common.Builders;

namespace Guppy.Core.Common.Extensions
{
    public static class GuppyScopeFilterBuilderExtensions
    {
        public static GuppyScopeFilterBuilder RequireScopeVariable<TVariable>(this GuppyScopeFilterBuilder filter, TVariable value)
            where TVariable : IScopeVariable
        {
            return filter.Require(builder => builder.GetScopeVariable<TVariable>()?.Matches(value) ?? false);
        }

        public static GuppyScopeFilterBuilder RequireEnvironmentVariable<TVariable>(this GuppyScopeFilterBuilder filter, TVariable value)
            where TVariable : IEnvironmentVariable
        {
            return filter.Require(builder => builder.EnvironmentVariables.Matches(value));
        }
    }
}
