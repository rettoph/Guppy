using Guppy.Core.Common;
using Guppy.Core.Common.Extensions;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Filters;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Game.Graphics.Common.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterGraphicsEnabledFilter(this IGuppyScopeBuilder builder, Type serviceType, bool value)
        {
            return builder.RegisterFilter(new StateServiceFilter<bool>(
                serviceType: serviceType,
                key: StateKey<bool>.Create(GraphicsStateKeys.GraphicsEnabled),
                value: value));
        }

        public static IGuppyScopeBuilder RegisterGraphicsEnabledFilter<TService>(this IGuppyScopeBuilder builder, bool value)
        {
            return builder.RegisterFilter(new StateServiceFilter<bool>(
                serviceType: typeof(TService),
                key: StateKey<bool>.Create(GraphicsStateKeys.GraphicsEnabled),
                value: value));
        }
    }
}