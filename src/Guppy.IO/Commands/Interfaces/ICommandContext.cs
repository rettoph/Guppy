using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public interface ICommandContext : ICommandGroupContext
    {
        /// <summary>
        /// An array of all arguments this particular command expects.
        /// All input string will be parsed according to the ArgContext
        /// Type converter.
        /// </summary>
        ArgContext[] Arguments { get; }

        /// <summary>
        /// Convert a parsed argument value 
        /// pair into a single useable object
        /// to push through the command pipeline.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Object BuildData(IReadOnlyDictionary<String, Object> args);
    }
}
