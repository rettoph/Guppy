using Guppy.Attributes;
using Guppy.CommandLine.Builders;
using Guppy.CommandLine.Interfaces;
using Guppy.CommandLine.Services;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ServiceLoaders
{
    [AutoLoad]
    internal sealed class CommandsServiceLoader : ICommandServiceLoader
    {
        public void RegisterCommands(CommandServiceBuilder commands)
        {
            commands.RegisterCommand("network")
                .SetDescription("Interact with current network info.")
                .AddSubCommand("users", users =>
                {
                    users.SetDescription("Investigate user info")
                        .AddOption(new Option<Int32?>("id", "Specific user Id")
                        {
                            IsRequired = false
                        });
                });
        }
    }
}
