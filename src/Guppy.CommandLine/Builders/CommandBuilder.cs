using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;

namespace Guppy.CommandLine.Builders
{
    public sealed class CommandBuilder
    {
        public readonly Type Type;
        public readonly CommandDefinition Definition;
        public CommandDefinition Parent { get; set; }

        #region Constructor
        public CommandBuilder(Type type)
        {
            typeof(CommandDefinition).ValidateAssignableFrom(type);

            this.Type = type;
            this.Definition = Activator.CreateInstance(type) as CommandDefinition;
        }
        #endregion

        #region SetDescription Methods
        public CommandBuilder SetParent(CommandDefinition parent)
        {
            this.Parent = parent;

            return this;
        }
        #endregion

        #region Build Methods
        internal Command Build()
        {
            var command = new Command(this.Definition.Name, this.Definition.Description);

            foreach (Option option in this.Definition.Options)
            {
                command.AddOption(option);
            }

            foreach (Argument argument in this.Definition.Arguments)
            {
                command.AddArgument(argument);
            }

            foreach (String alias in this.Definition.Aliases)
            {
                command.AddAlias(alias);
            }

            return command;
        }
        #endregion
    }
}
