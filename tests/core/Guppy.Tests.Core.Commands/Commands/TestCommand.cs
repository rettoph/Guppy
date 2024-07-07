using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Messaging.Common;

namespace Guppy.Tests.Core.Commands.Commands
{
    [Command]
    public class TestCommand : Message<TestCommand>, ICommand
    {
        public enum ValueEnum
        {
            TestValueA = 0,
            TestValueB = 1,
            TestValueC = 2,
            TestValueD = 3,
        }

        [Argument]
        public ValueEnum Value { get; set; }
    }
}
