using System.CommandLine;
using Guppy.Core.Commands.Common;
using Guppy.Core.Commands.Common.Serialization.Commands;
using Guppy.Core.Commands.Common.Services;
using Guppy.Core.Commands.Serialization.Commands;
using Guppy.Core.Commands.Services;
using Guppy.Core.Common;
using Guppy.Tests.Common.Mocks;
using Moq;

namespace Guppy.Tests.Core.Commands.Common
{
    public static class CommandServiceBuilder
    {
        private static readonly ICommandTokenConverter[] _defaultConverters = [
            new NullableEnumTokenConverter()
        ];

        public static ICommandService Build(ICommand[] commands, ICommandTokenConverter[]? customConverters = null)
        {
            ICommandTokenConverter[] converts = customConverters is null ? _defaultConverters : [.. _defaultConverters, .. customConverters];
            IFiltered<ICommandTokenConverter> converters = new MockFiltered<ICommandTokenConverter>(converts);
            ICommandTokenService tokenService = new CommandTokenService(converters);

            Mock<IConsole> console = new();

            ICommandService commandService = new CommandService(new MockFiltered<ICommand>(commands), tokenService, console.Object);

            return commandService;
        }
    }
}