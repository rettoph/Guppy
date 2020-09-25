using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public interface ICommandContext
    {
        /// <summary>
        /// A human readble description of what the current command
        /// physically does.
        /// </summary>
        String Description { get; }

        /// <summary>
        /// A full list of all possible arguments
        /// this command may recieve.
        /// </summary>
        ArgContext[] Arguments { get; }

        Object GetOutput(Dictionary<String, Object> args);
    }
}
