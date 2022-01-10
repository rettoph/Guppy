using System;
using System.Collections.Generic;
using System.Text;
using Guppy.CommandLine.Services;
using System.CommandLine;

namespace Guppy.EntityComponent.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Return the static CommandsService.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static CommandService GetCommands(this ServiceProvider provider)
                => provider.GetService<CommandService>();

        /// <summary>
        /// Automatically return a command directly from the
        /// ServiceProvider's Command service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Command GetCommand<TCommand>(this ServiceProvider provider)
            where TCommand : class
                => provider.GetService<CommandService>().Get<TCommand>();

        /// <summary>
        /// Automatically return a command directly from the
        /// ServiceProvider's Command service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static Command GetCommand(this ServiceProvider provider, String command)
                => provider.GetService<CommandService>().Get(command);
    }
}
