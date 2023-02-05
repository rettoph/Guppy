using Guppy.Attributes;
using Guppy.MonoGame.Services;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;

namespace Guppy.MonoGame.Definitions
{
    [Service<ICommandDefinition>(ServiceLifetime.Singleton, true)]
    public interface ICommandDefinition
    {
        Type? Parent { get; }
        string Name { get; }
        string? Description { get; }
        string[] Aliases { get; }
        public IEnumerable<Option> Options { get; }
        public IEnumerable<Argument> Arguments { get; }

        Command BuildCommand(ICommandService commands);
    }
}
