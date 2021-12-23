using Guppy.CommandLine.Builders;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.Interfaces
{
    public interface ICommandLoader : IGuppyLoader
    {
        void RegisterCommands(CommandServiceBuilder commands);
    }
}
