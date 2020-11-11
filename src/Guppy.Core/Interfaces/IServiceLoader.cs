using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;

namespace Guppy.Interfaces
{
    public interface IServiceLoader
    {
        void RegisterServices(ServiceCollection services);
        void ConfigureProvider(ServiceProvider provider);
    }
}
