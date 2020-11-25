﻿using Microsoft.Xna.Framework.Input;
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
        /// An array of button states and their command to run
        /// </summary>
        public (ButtonState state, String command)[] Commands { get; set; }
    }
}