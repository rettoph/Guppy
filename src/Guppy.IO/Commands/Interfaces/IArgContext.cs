using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public interface IArgContext : ICommandComponentContext
    {
        /// <summary>
        /// The expected data type for this argument.
        /// </summary>
        ArgType Type { get; }

        /// <summary>
        /// Alternative aliases this argument is known by.
        /// </summary>
        Char[] Aliases { get; }

        /// <summary>
        /// Whether or not this particular argument is required.
        /// </summary>
        Boolean Required { get; }
    }
}
