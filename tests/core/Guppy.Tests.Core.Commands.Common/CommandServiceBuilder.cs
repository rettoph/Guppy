﻿using Guppy.Core.Commands.Common.Contexts;
using Guppy.Core.Commands.Common.Serialization.Commands;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Serialization.Commands;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common;
using Guppy.Tests.Common;
using Guppy.Tests.Common.Mocks;
using Moq;
using System.CommandLine;

namespace Guppy.Tests.Core.Commands.Common
{
    public static class CommandServiceBuilder
    {
        private static ICommandTokenConverter[] DefaultConverters = [
            new NullableEnumTokenConverter()
        ];

        public static ICommandService Build(Type[] commandTypes, ICommandTokenConverter[]? customConverters = null)
        {
            ICommandTokenConverter[] converts = customConverters is null ? DefaultConverters : DefaultConverters.Concat(customConverters).ToArray();
            IFiltered<ICommandTokenConverter> converters = new MockFiltered<ICommandTokenConverter>(converts);
            ICommandTokenService tokenService = new CommandTokenService(converters);

            IEnumerable<ICommandContext> commands = commandTypes.Select(x => CommandContext.Create(x));
            IMock<IConsole> console = MockBuilder<IConsole>.Create().Build();

            ICommandService commandService = new CommandService(commands, tokenService, console.Object);

            return commandService;
        }
    }
}
