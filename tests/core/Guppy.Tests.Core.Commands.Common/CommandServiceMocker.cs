using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Commands.Extensions;
using Guppy.Core.Commands.Services;
using Guppy.Core.Messaging.Common;
using Guppy.Tests.Common;

namespace Guppy.Tests.Core.Commands.Common
{
    public class CommandServiceMocker<TCommand> : GuppyRootMocker<CommandServiceMocker<TCommand>, CommandService>
        where TCommand : class, ICommand
    {
        public IMessageBus MessageBus => this.root.Resolve<IMessageBus>();
        public CommandService CommandService => this.root.Resolve<CommandService>();

        public CommandServiceMocker() : base()
        {
            this.Register(builder =>
            {
                builder.RegisterCoreCommandServices();
                builder.RegisterCommand<TCommand>();
            });
        }
    }
}