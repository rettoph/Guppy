using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Loaders
{
    public interface IAssemblyLoader
    {
        void ConfigureAssemblies(IAssemblyProvider assemblies);
    }
}
