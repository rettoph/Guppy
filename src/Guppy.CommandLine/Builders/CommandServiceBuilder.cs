using Guppy.CommandLine.Arguments;
using Guppy.CommandLine.Interfaces;
using Guppy.CommandLine.Services;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.Builders
{
    public sealed class CommandServiceBuilder
    {
        private Dictionary<Type, CommandBuilder> _commands;

        internal CommandServiceBuilder()
        {
            _commands = new Dictionary<Type, CommandBuilder>();
        }

        /// <summary>
        /// Import a class structure layout and build correlating
        /// commands based on it.
        /// </summary>
        /// <param name="type"></param>
        public void ImportCommandDefinition(Type type)
        {
            this.ImportCommandDefinition(type, null);
        }

        /// <summary>
        /// Import a class structure layout and build correlating
        /// commands based on it.
        /// </summary>
        /// <param name="type"></param>
        public void ImportCommandDefinition<TCommand>()
            where TCommand : CommandDefinition
        {
            this.ImportCommandDefinition(typeof(TCommand), null);
        }

        private void ImportCommandDefinition(Type type, Type defaultParentType)
        {
            typeof(CommandDefinition).ValidateAssignableFrom(type);

            if (_commands.ContainsKey(type))
            { // This type has already been imported...
                return;
            }

            // Attempt to load the parent...
            CommandDefinition parent = default;
            if (type.TryGetAttribute(out CommandParentAttribute parentAttribute) || defaultParentType is not null)
            { // Ensure that the parent type has already been imported...
                this.ImportCommandDefinition(parentAttribute?.Type ?? defaultParentType);
                parent = _commands[parentAttribute?.Type ?? defaultParentType].Definition;
            }

            // Add the requested definition
            var builder = new CommandBuilder(type);
            builder.SetParent(parent);
            _commands.Add(type, builder);

            foreach(Type subType in type.GetNestedTypes())
            {
                if(typeof(CommandDefinition).IsAssignableFrom(subType))
                {
                    this.ImportCommandDefinition(subType, type);
                }
            }
        }

        public CommandService Build()
        {
            return new CommandService(_commands.Build());
        }
    }
}
