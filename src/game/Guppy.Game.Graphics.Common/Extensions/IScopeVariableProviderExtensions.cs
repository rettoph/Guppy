using Guppy.Core.Common;
using Guppy.Core.Common.Providers;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Core.Network.Common.Extensions
{
    public static class IScopeVariableProviderExtensions
    {
        public static bool GetGraphicsEnabled(this IGuppyVariableProvider<IScopeVariable> provider)
        {
            return provider.TryGet<GuppyGraphicsVariables.Scope.GraphicsEnabled>(out var variable) ? variable.Value : throw new NotImplementedException();
        }
    }
}
