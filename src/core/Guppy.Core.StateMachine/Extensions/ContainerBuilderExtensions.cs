using Autofac;
using Guppy.Core.Common.Extensions.Autofac;
using Guppy.Core.StateMachine.Common.Services;
using Guppy.StateMachine.Services;

namespace Guppy.Core.StateMachine.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCoreStateMachineServices(this ContainerBuilder builder)
        {
            if (builder.HasTag(nameof(RegisterCoreStateMachineServices)))
            {
                return builder;
            }

            builder.RegisterType<StateService>().As<IStateService>().InstancePerLifetimeScope();

            return builder.AddTag(nameof(RegisterCoreStateMachineServices));
        }
    }
}