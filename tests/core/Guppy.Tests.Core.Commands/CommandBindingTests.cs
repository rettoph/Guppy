using System.CommandLine;
using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Extensions;
using Guppy.Core.Common.Attributes;
using Guppy.Core.Messaging.Common.Enums;
using Guppy.Tests.Core.Commands.Common;
using Moq;

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

        public class EnumArgumentTestCommand : Command<EnumArgumentTestCommand>, ICommand
        {
            [Argument]
            public ValueEnum Value { get; set; }
        }

        public class NullableEnumArgumentTestCommand : Command<NullableEnumArgumentTestCommand>, ICommand
        {
            [Argument]
            public ValueEnum? Value { get; set; }
        }

        public class EnumOptionTestCommand : Command<EnumOptionTestCommand>, ICommand
        {
            [Option]
            public ValueEnum Value { get; set; }
        }

        public class NullableEnumOptionTestCommand : Command<NullableEnumOptionTestCommand>, ICommand
        {
            [Option(required: false)]
            public ValueEnum? Value { get; set; }
        }

        public class TestCommandSubscriber<TCommand> : ICommandSubscriber<TCommand>
            where TCommand : ICommand
        {
            [SequenceGroup<SubscriberSequenceGroupEnum>(SubscriberSequenceGroupEnum.Process)]
            public virtual void Process(TCommand message) { }
        }

        [Fact]
        public void InvokeCommand_EnumArgument()
        {
            using var mocker = new CommandServiceMocker<EnumArgumentTestCommand>();

            Mock<TestCommandSubscriber<EnumArgumentTestCommand>> subscriber = new();
            mocker.MessageBus.Subscribe(subscriber.Object);

            // Attempt to invoke test command
            mocker.CommandService.Invoke($"{nameof(EnumArgumentTestCommand).ToCommandName()} TestValueC");

            // Ensure bus is flushed
            mocker.MessageBus.Flush();

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    It.Is<EnumArgumentTestCommand>(
                        (instance, type) => ((EnumArgumentTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );
        }

        [Fact]
        public void InvokeCommand_NullableEnumArgument()
        {
            using var mocker = new CommandServiceMocker<NullableEnumArgumentTestCommand>();

            Mock<TestCommandSubscriber<NullableEnumArgumentTestCommand>> subscriber = new();
            mocker.MessageBus.Subscribe(subscriber.Object);

            // Attempt to invoke test command with a value
            mocker.CommandService.Invoke($"{nameof(NullableEnumArgumentTestCommand).ToCommandName()} TestValueC");

            // Ensure bus is flushed
            mocker.MessageBus.Flush();

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    It.Is<NullableEnumArgumentTestCommand>(
                        (instance, type) => ((NullableEnumArgumentTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );

            // Attempt to invoke test command with a null value
            mocker.CommandService.Invoke($"{nameof(NullableEnumArgumentTestCommand).ToCommandName()}");

            // Ensure bus is flushed
            mocker.MessageBus.Flush();

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    It.Is<NullableEnumArgumentTestCommand>(
                        (instance, type) => ((NullableEnumArgumentTestCommand)instance).Value == null
                    )
                )
            );
        }

        [Fact]
        public void InvokeCommand_EnumOption()
        {
            using var mocker = new CommandServiceMocker<EnumOptionTestCommand>();

            Mock<TestCommandSubscriber<EnumOptionTestCommand>> subscriber = new();
            mocker.MessageBus.Subscribe(subscriber.Object);

            // Attempt to invoke test command
            mocker.CommandService.Invoke($"{nameof(EnumOptionTestCommand).ToCommandName()} {nameof(EnumOptionTestCommand.Value).ToOptionName()} TestValueC");

            // Ensure bus is flushed
            mocker.MessageBus.Flush();

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    It.Is<EnumOptionTestCommand>(
                        (instance, type) => ((EnumOptionTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );
        }

        [Fact]
        public void InvokeCommand_NullableEnumOption()
        {
            using var mocker = new CommandServiceMocker<NullableEnumOptionTestCommand>();

            Mock<TestCommandSubscriber<NullableEnumOptionTestCommand>> subscriber = new();
            mocker.MessageBus.Subscribe(subscriber.Object);

            // Attempt to invoke test command with a value
            mocker.CommandService.Invoke($"{nameof(NullableEnumOptionTestCommand).ToCommandName()} {nameof(EnumOptionTestCommand.Value).ToOptionName()} TestValueC");

            // Ensure bus is flushed
            mocker.MessageBus.Flush();

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    It.Is<NullableEnumOptionTestCommand>(
                        (instance, type) => ((NullableEnumOptionTestCommand)instance).Value == ValueEnum.TestValueC
                    )
                )
            );

            // Attempt to invoke test command with a null value
            mocker.CommandService.Invoke($"{nameof(NullableEnumOptionTestCommand).ToCommandName()}");

            // Ensure bus is flushed
            mocker.MessageBus.Flush();

            // Ensure subscriber ran as expected
            subscriber.Verify(
                x => x.Process(
                    It.Is<NullableEnumOptionTestCommand>(
                        (instance, type) => ((NullableEnumOptionTestCommand)instance).Value == null
                    )
                )
            );
        }
    }
}