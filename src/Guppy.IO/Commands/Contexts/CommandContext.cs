using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Contexts
{
    public struct CommandContext
    {
        /// <summary>
        /// Primary identifying word used to select this
        /// particular command.
        /// </summary>
        public String Word { get; set; }

        /// <summary>
        /// A human readble description of what the current command
        /// physically does.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// A full list of all possible arguments
        /// this command may recieve.
        /// </summary>
        public ArgContext[] Arguments { get; set; }

        /// <summary>
        /// Internal invokable commands that are children
        /// of the current command word.
        /// </summary>
        public CommandContext[] SubCommands { get; set; }

        public CommandContext(String word, String description, ArgContext[] arguments, CommandContext[] subCommands)
        {
            this.Word = word;
            this.Description = description;
            this.Arguments = arguments;
            this.SubCommands = subCommands;
        }
    }
}
