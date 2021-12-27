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
                Type parentType = parentAttribute?.Type ?? defaultParentType;

                if(parentType is not null)
                {
                    if (_commands.TryGetValue(parentType, out CommandBuilder parentCommentBuilder))
                    {
                        parent = parentCommentBuilder.Definition;
                    }
                    else
                    {
                        this.ImportCommandDefinition(parentType);
                        parent = _commands[parentAttribute?.Type ?? defaultParentType].Definition;
                    }
                }
                
                
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
            var commands = new CommandService(_commands.Build());

            // We can go through and create all the command handlers now. Its not ideal but it works
            foreach(CommandBuilder builder in _commands.Values)
            {
                commands.Get(builder.Type).Handler = builder.Definition.CreateCommandHandler(commands);
            }

            return commands;
        }
    }
}
