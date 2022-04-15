using Guppy.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Threading.Loaders
{
    public interface IBusLoader : IGuppyLoader
    {
        void ConfigureBus(IBusMessageCollection bus);
    }
}
