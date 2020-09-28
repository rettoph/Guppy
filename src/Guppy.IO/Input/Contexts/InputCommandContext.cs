using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Input.Contexts
{
    public struct InputCommandContext
    {
        /// <summary>
        /// A unique human readable name for this
        /// specific InputCommand.
        /// </summary>
        public String Handle { get; set; }

        /// <summary>
        /// The input type that will trigger a command invocation.
        /// </summary>
        public InputType DefaultInput { get; set; }

        /// <summary>
        /// The command that should be parsed
        /// & executed on input.
        /// </summary>
        public String Command { get; set; }

        /// <summary>
        /// A list of all input states that can invoke the
        /// current InputCommand.
        /// </summary>
        public ButtonState[] States { get; set; }
    }
}
