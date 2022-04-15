using Guppy.Loaders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Initializers
{
    public interface IGuppyInitializer
    {
        void Initialize(AssemblyHelper assemblies, IServiceCollection services, IEnumerable<IGuppyLoader> loaders);
    }
}
