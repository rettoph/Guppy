using Guppy.Resources.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface IInitializableResource
    {
        void Initialize(IResourceProvider resources);
    }
}
