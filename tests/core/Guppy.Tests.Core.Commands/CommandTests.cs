using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Common.TokenPropertySetters;
using Guppy.Core.Commands.Services;
using Guppy.Core.Messaging.Common;
using Guppy.Tests.Common;
using Guppy.Tests.Core.Commands.Commands;
using Moq;
using SCL = System.CommandLine;

namespace Guppy.Tests.Core.Commands
{
    public class CommandTests
    {
        [Fact]
        public void Test1()
        {
            IMock<IBroker<ICommand>> commandBroker = MockBuilder<IBroker<ICommand>>.Create().Build();


            IEnumerable<Command> commands = Command.Create(typeof(TestCommand)).Yield();
            IMock<SCL.IConsole> console = MockBuilder<SCL.IConsole>.Create().Build();

            ICommandService commandService = new CommandService(commands, Enumerable.Empty<ITokenPropertySetter>(), console.Object);


        }
    }
}