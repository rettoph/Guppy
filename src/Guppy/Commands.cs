using Guppy.Attributes;
using Guppy.CommandLine;
using Guppy.CommandLine.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public static class Commands
    {
        [AutoLoad]
        [CommandParent(typeof(CommandLine.Commands.Guppy))]
        public class Scenes : CommandDefinition
        {

        }
    }
}
