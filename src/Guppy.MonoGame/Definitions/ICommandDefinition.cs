using Guppy.Common;
using Guppy.MonoGame.Services;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Definitions
{
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
