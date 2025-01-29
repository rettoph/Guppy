using Guppy.Core.Common.Constants;
using Guppy.Core.Common.Enums;

namespace Guppy.Core.Common.Extensions
{
    public static class IGuppyScopeExtensions
    {
        public static GuppyScopeTypeEnum GetScopeType(this IGuppyScope scope)
        {
            return scope.Variables.Get<GuppyVariables.Scope.ScopeType>().Value;
        }
    }
}
