using Guppy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IServiceLoader
    {
        void ConfigureServices(ServiceCollection services);
        void ConfigureProvider(ServiceProvider provider);
    }
}
