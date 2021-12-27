using Guppy.Attributes;
using Guppy.CommandLine;
using Guppy.CommandLine.Arguments;
using Guppy.CommandLine.Services;
using Guppy.Messages;
using Guppy.Threading.Utilities;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class Commands
    {
        [AutoLoad]
        [CommandParent(typeof(CommandLine.Commands.Guppy))]
        public class Debug : CommandDefinition
        {
            public override Option[] Options => new[]
            {
                new Option<Boolean?>("-resetfps", "Reset the fps tracker info"),
            };

            public override ICommandHandler CreateCommandHandler(CommandService commands)
            {
                return CommandHandler.Create<Boolean?>((resetfps) =>
                {
                    commands.Process(new DebugFpsCommand()
                    {
                        ResetFps = resetfps ?? false,
                    });
                });
            }
        }
    }
}
