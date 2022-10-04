using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    public interface IAssemblyLoader : IGuppyLoader
    {
        void ConfigureAssemblies(IAssemblyProvider assemblies);
    }
}
