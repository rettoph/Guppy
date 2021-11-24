using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;

namespace Guppy.Interfaces
{
    public interface IServiceLoader
    {
        void RegisterServices(AssemblyHelper assemblyHelper, GuppyServiceCollection services);
        void ConfigureProvider(GuppyServiceProvider provider);
    }
}
