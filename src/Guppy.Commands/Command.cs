using Guppy.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands
{
    public sealed class Command
    {
        public readonly Type Type;
        public readonly Type? Parent;
        public readonly string Name;
        public readonly string? Description;
        public readonly Option[] Options;

        public Command(
            Type type,
            Type? parent, 
            string name, 
            string? description,
            Option[] options)
        {
            this.Type = type;
            this.Parent = parent;
            this.Name = name;
            this.Description = description;
            this.Options = options;
        }
    }
}
