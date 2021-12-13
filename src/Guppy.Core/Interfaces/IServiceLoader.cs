using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using Guppy.DependencyInjection.Builders;

namespace Guppy.Interfaces
{
    public interface IServiceLoader
    {
        void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceProviderBuilder services);
        void ConfigureProvider(GuppyServiceProvider provider);
    }
}
