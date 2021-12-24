using Guppy.Attributes;
using Guppy.CommandLine;
using Guppy.CommandLine.Arguments;
using Guppy.CommandLine.Interfaces;
using Guppy.CommandLine.Services;
using Guppy.Network.Messages.Commands;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public static class Commands
    {
        [AutoLoad]
        [CommandParent(typeof(CommandLine.Commands.Guppy))]
        public class Network : CommandDefinition
        {
            public override String Description => "Guppy.Network commands.";

            public class Users : CommandDefinition
            {
                public override String Description => "Guppy.Network User commands.";

                public override Option[] Options => new[] 
                {
                    new Option<Int32?>("id", "Specific user Id")
                };

                public override ICommandHandler CreateCommandHandler(CommandService commands)
                {
                    return CommandHandler.Create<Int32?>((id) =>
                    {
                        commands.Process(new GuppyNetworkUsersCommand()
                        {
                            Id = id,
                        });
                    });
                }
            }
        }
    }
}
