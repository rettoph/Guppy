﻿using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using Guppy.CommandLine.Services;
using System.CommandLine;

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
        public static Command GetCommand(this GuppyServiceProvider provider, String command)
            => provider.GetService<CommandService>().Get(command);

        /// <summary>
        /// Automatically return a command directly from the
        /// ServiceProvider's Command service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static TCommand GetCommand<TCommand>(this GuppyServiceProvider provider, String command)
            where TCommand : class, ICommand
                => provider.GetService<CommandService>().Get<TCommand>(command);
    }
}
