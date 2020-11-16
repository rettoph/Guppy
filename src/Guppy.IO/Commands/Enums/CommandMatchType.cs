using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Enums
{
    public enum CommandMatchType
    {
        /// <summary>
        /// Indicates that the recieved command is not
        /// a perfect match (thus render the help command)
        /// </summary>
        Incomplete,
        
        /// <summary>
        /// Indicates that the resiceved command is
        /// the command requested.
        /// </summary>
        Complete
    }
}
