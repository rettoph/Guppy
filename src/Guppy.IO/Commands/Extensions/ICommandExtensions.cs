using Guppy.IO.Commands.Enums;
using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using Guppy.IO.Commands.Structs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Extensions
{
    public static class ICommandExtensions
    {
        public static void TryRemove(this ICommandBase parent, ICommand child)
            => parent.TryRemove(child.Word);

        public static CommandMatch TryFindMatch(this ICommand command, String[] input, ref Int32 position)
        {
            position++;

            if (position == input.Length || input[position][0] == CommandService.ArgumentIdentifier)
            { // We have hit the first argument... return the current command.
                return CommandMatch.Complete(command);
            }
            else if(command.Commands.ContainsKey(input[position]))
            { // There is a sub segment requested...
                return command[input[position]].TryFindMatch(input, ref position);
            }
            else
                return CommandMatch.Incomplete(command);
        }
    }
}
