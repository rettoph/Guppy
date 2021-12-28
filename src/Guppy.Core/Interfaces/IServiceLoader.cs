using Guppy.EntityComponent.DependencyInjection.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ServiceLoaders
{
    public interface IServiceLoader : IGuppyLoader
    {
        void RegisterServices(AssemblyHelper assemblyHelper, ServiceProviderBuilder services);
    }
}
