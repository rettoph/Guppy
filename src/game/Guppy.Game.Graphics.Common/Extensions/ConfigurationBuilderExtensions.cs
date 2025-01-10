using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common;
using Guppy.Core.StateMachine.Common.Filters;
using Guppy.Game.Graphics.Common.Constants;

namespace Guppy.Game.Graphics.Common.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static ContainerBuilder RegisterGraphicsEnabledFilter(this ContainerBuilder builder, Type serviceType, bool value) => builder.RegisterFilter(new StateServiceFilter<bool>(
                serviceType: serviceType,
                key: StateKey<bool>.Create(GraphicsStateKeys.GraphicsEnabled),
                value: value));

        public static ContainerBuilder RegisterGraphicsEnabledFilter<TService>(this ContainerBuilder builder, bool value) => builder.RegisterFilter(new StateServiceFilter<bool>(
                serviceType: typeof(TService),
                key: StateKey<bool>.Create(GraphicsStateKeys.GraphicsEnabled),
                value: value));
    }
}