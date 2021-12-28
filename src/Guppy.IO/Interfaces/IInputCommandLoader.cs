using Guppy.Interfaces;
using Guppy.IO.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ServiceLoaders
{
    public interface IInputCommandLoader : IGuppyLoader
    {
        void RegisterInputCommands(InputCommandServiceBuilder inputCommands);
    }
}
