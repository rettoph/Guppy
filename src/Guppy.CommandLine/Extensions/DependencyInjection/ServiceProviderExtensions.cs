using System;
using System.Collections.Generic;
using System.Text;
using Guppy.CommandLine.Services;
using System.CommandLine;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.CommandLine.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
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

        /// <summary>
        /// Automatically return a command directly from the
        /// ServiceProvider's Command service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static TCommand GetCommand<TCommand>(this ServiceProvider provider, String command)
            where TCommand : class, ICommand
                => provider.GetService<CommandService>().Get<TCommand>(command);
    }
}
