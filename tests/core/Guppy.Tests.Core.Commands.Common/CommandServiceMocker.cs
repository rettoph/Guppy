using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Commands.Extensions;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common.Enums;
using Guppy.Core.Messaging.Common;
using Guppy.Tests.Common;

namespace Guppy.Tests.Core.Commands.Common
{
    public class CommandServiceMocker<TCommand> : GuppyScopeMocker<CommandServiceMocker<TCommand>, CommandService>
        where TCommand : class, ICommand
    {
        public IMessageBus MessageBus => this.scope.Resolve<IMessageBus>();
        public CommandService CommandService => this.scope.Resolve<CommandService>();

        public CommandServiceMocker() : base(GuppyScopeTypeEnum.Root)
        {
            this.Register(builder =>
            {
                builder.RegisterCoreCommandServices();
                builder.RegisterCommand<TCommand>();
            });
        }
    }
}