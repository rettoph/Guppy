using Guppy.IO.Commands.Interfaces;
using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Extensions
{
    public static class ICommandGroupContextParentExtensions
    {
        public static Command Build(this ICommandGroupContextParent context, String[] args, Byte position)
        {


            if(position == args.Length || args[position][0] == CommandService.ArgumentIdentifier)
            { // An argument was just recieved or this is the last group item...
                return ((ICommandContext)context).Build(args, position);
            }
            else
            { // A nested group is being called...
                return context.Groups[args[position]].Build(args, ++position);
            }
        }
    }
}
