using Guppy.CommandLine.Services;
using Guppy.EntityComponent.DependencyInjection.Builders;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.ServiceLoaders
{
    internal sealed class CommandsServiceLoader : IServiceLoader
    {
        public void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services)
        {
            services.RegisterSetup<CommandService>()
                .SetMethod((commands, _, _) =>
                {
                    var network = new Command("network", "Interact with current network info.")
                    {
                    };

                    var user = new Command("user", "Investigate a current user")
                    {
                        // new 
                    };

                    network.AddCommand(user);

                    commands.Get().Add(network);
                    commands.Get().Add(new Command("test", "This is a test command")
                    {
                        new Argument<string>("input", "This is a test argument"),
                    });
                });
        }
    }
}
