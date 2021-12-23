using Guppy.Attributes;
using Guppy.CommandLine.Arguments;
using Guppy.CommandLine.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine
{
    public class Commands
    {
        [AutoLoad]
        public class Guppy : CommandDefinition
        {
            public override String Name => "guppy";

            public override String Description => "Guppy commands.";

        }
    }
}
