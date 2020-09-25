using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Interfaces
{
    public struct Command
    {
        /// <summary>
        /// The current command's context
        /// </summary>
        public ICommandContext Context { get; internal set; }

        /// <summary>
        /// The parsed command data.
        /// </summary>
        public Object Data { get; internal set; }
    }
}
