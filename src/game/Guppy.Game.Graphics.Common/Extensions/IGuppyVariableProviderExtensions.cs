using Guppy.Core.Common;
using Guppy.Core.Common.Providers;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IGuppyVariableProviderExtensions
    {
        public static bool GetGraphicsEnabled(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.GetVariable<GuppyGraphicsVariables.Scope.GraphicsEnabled>()?.Value ?? false;
        }
    }
}
