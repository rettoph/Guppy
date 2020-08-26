using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;

namespace Guppy.Interfaces
{
    public interface IServiceLoader
    {
        void ConfigureServices(ServiceCollection services);
        void ConfigureProvider(ServiceProvider provider);
    }
}
