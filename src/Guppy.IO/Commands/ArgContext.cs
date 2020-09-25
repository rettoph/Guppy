using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    /// <inheritdoc />
    public struct ArgContext : IArgContext
    {
        /// <inheritdoc />
        public ArgType Type { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public char[] Aliases { get; }

        /// <inheritdoc />
        public bool Required { get; }

        public ArgContext(ArgType type, string name, string description, char[] aliases, bool required)
        {
            this.Type = type;
            this.Name = name;
            this.Description = description;
            this.Aliases = aliases;
            this.Required = required;
        }
    }
}
