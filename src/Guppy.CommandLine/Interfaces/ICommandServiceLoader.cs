using Guppy.CommandLine.Builders;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.Interfaces
{
    public interface ICommandServiceLoader : IGuppyLoader
    {
        void RegisterCommands(CommandServiceBuilder commands);
    }
}
