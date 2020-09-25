using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands.Contexts
{
    public class HelloWorldContext : CommandContext
    {
        public override ArgContext[] Arguments => new ArgContext[] {
            new ArgContext(ArgType.Boolean, "test", "This is a test argument", new Char[] { 't' }, true)
        };

        public override string Name => "helloworld";

        public override string Description => "Basic hello world command";

        public override object BuildData(IReadOnlyDictionary<string, object> args)
        {
            return null;
        }
    }
}
