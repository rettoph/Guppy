using Guppy.CommandLine.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.CommandLine.Interfaces
{
    public interface ICommandServiceLoader
    {
        void RegisterCommands(CommandServiceBuilder commands);
    }
}
