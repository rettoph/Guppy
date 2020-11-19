using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Contexts
{
    public struct ArgContext
    {
        /// <summary>
        /// The expected data type for this argument.
        /// </summary>
        public ArgType Type { get; set; }

        /// <summary>
        /// The primary non aliase identifier for
        /// this argument.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// A huma readable explanation of what this argument does.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Alternative aliases this argument is known by.
        /// </summary>
        public Char[] Aliases { get; set; }

        /// <summary>
        /// Whether or not this particular argument is required.
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// The default input value to parse if non
        /// is recieved.
        /// 
        /// This is only applicable if required is false.
        /// </summary>
        public Func<Object> DefaultValue { get; set; }

        public ArgContext(ArgType type, string identifier, string description, Char[] aliases = null, bool required = false, Func<Object> defaultValue = null)
        {
            this.Type = type;
            this.Identifier = identifier;
            this.Description = description;
            this.Aliases = aliases;
            this.Required = required;
            this.DefaultValue = defaultValue;
        }
    }
}
