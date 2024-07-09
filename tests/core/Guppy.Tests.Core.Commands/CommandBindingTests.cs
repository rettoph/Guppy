using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Messaging.Common;
using Guppy.Tests.Core.Commands.Common;
using Moq;
using System.CommandLine;

namespace Guppy.Tests.Core.Commands
{
    public class CommandBindingTests
    {
        public enum ValueEnum
        {
            TestValueA = 0,
            TestValueB = 1,
            TestValueC = 2,
            TestValueD = 3,
        }

        public class EnumArgumentTestCommand : Message<EnumArgumentTestCommand>, ICommand
        {
            [Argument]
            public ValueEnum Value { get; set; }
        }

        public class NullableEnumArgumentTestCommand : Message<NullableEnumArgumentTestCommand>, ICommand
        {
            [Argument]
            public ValueEnum? Value { get; set; }
        }

        public class EnumOptionTestCommand : Message<EnumOptionTestCommand>, ICommand
        {
            [Option]
            public ValueEnum Value { get; set; }
        }

        public class NullableEnumOptionTestCommand : Message<NullableEnumOptionTestCommand>, ICommand
        {
            [Option(required: false)]
            public ValueEnum? Value { get; set; }
        }

        [Fact]
        public void InvokeCommand_EnumArgument()
        {
            ICommandService commandService = CommandServiceBuilder.Build([typeof(EnumArgumentTestCommand)]);

            Mock<ICommandSubscriber<EnumArgumentTestCommand>> subscriber = new Mock<ICommandSubscriber<EnumArgumentTestCommand>>();
            commandService.Subscribe(subscriber.Object);

            // Attempt to invoke test command
            commandService.Invoke($"{nameof(EnumArgumentTestCommand).ToCommandName()} TestValueC");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<EnumArgumentTestCommand>(
                        (instance, type) => ((EnumArgumentTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );
        }

        [Fact]
        public void InvokeCommand_NullableEnumArgument()
        {
            ICommandService commandService = CommandServiceBuilder.Build([typeof(NullableEnumArgumentTestCommand)]);

            Mock<ICommandSubscriber<NullableEnumArgumentTestCommand>> subscriber = new Mock<ICommandSubscriber<NullableEnumArgumentTestCommand>>();
            commandService.Subscribe(subscriber.Object);

            // Attempt to invoke test command with a value
            commandService.Invoke($"{nameof(NullableEnumArgumentTestCommand).ToCommandName()} TestValueC");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<NullableEnumArgumentTestCommand>(
                        (instance, type) => ((NullableEnumArgumentTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );

            // Attempt to invoke test command with a null value
            commandService.Invoke($"{nameof(NullableEnumArgumentTestCommand).ToCommandName()}");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<NullableEnumArgumentTestCommand>(
                        (instance, type) => ((NullableEnumArgumentTestCommand)instance).Value == null
                    )
                )
            );
        }

        [Fact]
        public void InvokeCommand_EnumOption()
        {
            ICommandService commandService = CommandServiceBuilder.Build([typeof(EnumOptionTestCommand)]);

            Mock<ICommandSubscriber<EnumOptionTestCommand>> subscriber = new Mock<ICommandSubscriber<EnumOptionTestCommand>>();
            commandService.Subscribe(subscriber.Object);

            // Attempt to invoke test command
            commandService.Invoke($"{nameof(EnumOptionTestCommand).ToCommandName()} {nameof(EnumOptionTestCommand.Value).ToOptionName()} TestValueC");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<EnumOptionTestCommand>(
                        (instance, type) => ((EnumOptionTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );
        }

        [Fact]
        public void InvokeCommand_NullableEnumOption()
        {
            ICommandService commandService = CommandServiceBuilder.Build([typeof(NullableEnumOptionTestCommand)]);

            Mock<ICommandSubscriber<NullableEnumOptionTestCommand>> subscriber = new Mock<ICommandSubscriber<NullableEnumOptionTestCommand>>();
            commandService.Subscribe(subscriber.Object);

            // Attempt to invoke test command with a value
            commandService.Invoke($"{nameof(NullableEnumOptionTestCommand).ToCommandName()} {nameof(EnumOptionTestCommand.Value).ToOptionName()} TestValueC");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<NullableEnumOptionTestCommand>(
                        (instance, type) => ((NullableEnumOptionTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );

            // Attempt to invoke test command with a null value
            commandService.Invoke($"{nameof(NullableEnumOptionTestCommand).ToCommandName()}");

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    in It.Ref<Guid>.IsAny,
                    It.Is<NullableEnumOptionTestCommand>(
                        (instance, type) => ((NullableEnumOptionTestCommand)instance).Value == null
                    )
                )
            );
        }
    }
}