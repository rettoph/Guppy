using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Filters;
using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.Common.Attributes
{
    public class SceneHasDebugWindowFilterAttribute : GuppyConfigurationAttribute
    {
        public readonly bool WhenHasDebugWindow;

        public SceneHasDebugWindowFilterAttribute(bool whenHasDebugWindow = true)
        {
            this.WhenHasDebugWindow = whenHasDebugWindow;
        }

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new StateServiceFilter<bool>(
                serviceType: classType,
                key: StateKey<bool>.Create(SceneConfigurationKeys.SceneHasDebugWindow),
                value: this.WhenHasDebugWindow));
        }
    }
}
