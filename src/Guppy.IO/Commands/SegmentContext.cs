using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    public struct SegmentContext
    {
        /// <summary>
        /// The human readable segment used to
        /// identify the current segment within
        /// a command chain.
        /// </summary>
        public String Identifier;

        /// <summary>
        /// The specific CommandContext instance
        /// to invoke when this segment is called.
        /// </summary>
        public ICommandContext Command;

        /// <summary>
        /// All children, if any, invocable within
        /// the current segment.
        /// </summary>
        public SegmentContext[] SubSegments;
    }
}
