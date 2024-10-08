using Autofac;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Filters;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Game.Graphics.Common.Attributes
{
    public class GraphicsEnabledAttribute(bool value = true) : GuppyConfigurationAttribute
    {
        public readonly bool Value = value;

        protected override void Configure(IContainer boot, ContainerBuilder builder, Type classType)
        {
            builder.RegisterFilter(new StateServiceFilter<bool>(
                serviceType: classType,
                key: StateKey<bool>.Create(GraphicsStateKeys.GraphicsEnabled),
                value: this.Value));
        }
    }
}
