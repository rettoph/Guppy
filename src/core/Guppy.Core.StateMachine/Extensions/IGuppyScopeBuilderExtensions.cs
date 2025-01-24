using Guppy.Core.Common;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common.Services;
using Guppy.StateMachine.Services;

namespace Guppy.Core.StateMachine.Extensions
{
    public static class IGuppyScopeBuilderExtensions
    {
        public static IGuppyScopeBuilder RegisterCoreStateMachineServices(this IGuppyScopeBuilder builder)
        {
            return builder.EnsureRegisteredOnce(nameof(RegisterCoreStateMachineServices), builder =>
            {
                builder.RegisterType<StateService>().As<IStateService>().InstancePerLifetimeScope();
            });
        }
    }
}