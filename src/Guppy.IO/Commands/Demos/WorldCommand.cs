using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Demos
{
    public class WorldCommand : ICommandContext
    {
        public string Description { get; } = "test";

        public ArgContext[] Arguments { get; } = new ArgContext[]
        {
            new ArgContext(ArgType.Boolean, "test", "this is a test arg")
        };

        public object GetOutput(Dictionary<string, object> args)
        {
            throw new NotImplementedException();
        }
    }
}
