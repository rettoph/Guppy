using Guppy.IO.Commands.Enums;
using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Structs
{
    public struct CommandMatch
    {
        public readonly CommandMatchType Type;
        public readonly ICommand Command;

        private CommandMatch(CommandMatchType type, ICommand command)
        {
            this.Type = type;
            this.Command = command;
        }

        public static CommandMatch Complete(ICommand command)
            => new CommandMatch(CommandMatchType.Complete, command);

        public static CommandMatch Incomplete(ICommand command = default)
            => new CommandMatch(CommandMatchType.Incomplete, command);

        public static CommandMatch Help(ICommand command = default)
            => new CommandMatch(CommandMatchType.Help, command);
    }
}
