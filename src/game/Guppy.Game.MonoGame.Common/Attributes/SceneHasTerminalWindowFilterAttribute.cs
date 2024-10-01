using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Filters;
using Guppy.Game.MonoGame.Common.Constants;

namespace Guppy.Game.MonoGame.Common.Attributes
{
    public class SceneHasTerminalWindowFilterAttribute(bool whenHasDebugWindow = true) : GuppyConfigurationAttribute
    {
        public readonly bool WhenHasTerminalWindow = whenHasDebugWindow;

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new StateServiceFilter<bool>(
                serviceType: classType,
                key: StateKey<bool>.Create(SceneConfigurationKeys.SceneHasTerminalWindow),
                value: this.WhenHasTerminalWindow));
        }
    }
}
