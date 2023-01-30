using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Loaders
{
    public interface IPackLoader
    {
        void Load(IPackProvider packs);
    }
}
