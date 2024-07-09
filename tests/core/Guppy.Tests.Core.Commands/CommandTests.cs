using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Messaging.Common;
using Guppy.Tests.Core.Commands.Common;
using Moq;
using System.CommandLine;

namespace Guppy.Tests.Core.Commands
{
    public class CommandTests
    {
        public enum ValueEnum
        {
            TestValueA = 0,
            TestValueB = 1,
            TestValueC = 2,
            TestValueD = 3,
        }

        public class EnumTestCommand : Message<EnumTestCommand>, ICommand
        {
            [Argument]
            public ValueEnum Value { get; set; }
        }

        public class NullableEnumTestCommand : Message<NullableEnumTestCommand>, ICommand
        {
            [Argument]
            public ValueEnum? Value { get; set; }
        }

        [Fact]
        public void InvokeCommand_EnumArgument()
        {
            ICommandService commandService = CommandServiceBuilder.Build([typeof(EnumTestCommand)]);

            Mock<ICommandSubscriber<EnumTestCommand>> subscriber = new Mock<ICommandSubscriber<EnumTestCommand>>();
            commandService.Subscribe(subscriber.Object);

            // Attempt to invoke test command
            commandService.Invoke("enumTestCommand TestValueC");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<EnumTestCommand>(
                        (instance, type) => ((EnumTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );
        }

        [Fact]
        public void InvokeCommand_NullableEnumArgument()
        {
            ICommandService commandService = CommandServiceBuilder.Build([typeof(NullableEnumTestCommand)]);

            Mock<ICommandSubscriber<NullableEnumTestCommand>> subscriber = new Mock<ICommandSubscriber<NullableEnumTestCommand>>();
            commandService.Subscribe(subscriber.Object);

            // Attempt to invoke test command with a value
            commandService.Invoke("nullableEnumTestCommand TestValueC");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<NullableEnumTestCommand>(
                        (instance, type) => ((NullableEnumTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );

            // Attempt to invoke test command with a null value
            commandService.Invoke("nullableEnumTestCommand");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<NullableEnumTestCommand>(
                        (instance, type) => ((NullableEnumTestCommand)instance).Value == null
                    )
                )
            );
        }
    }
}