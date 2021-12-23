using Guppy.Attributes;
using Guppy.CommandLine.Builders;
using Guppy.CommandLine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.IO.ServiceLoaders
{
    [AutoLoad]
    internal sealed class CommandServiceLoader : ICommandLoader
    {
        public void RegisterCommands(CommandServiceBuilder commands)
        {
            // throw new NotImplementedException();
        }
    }
}
