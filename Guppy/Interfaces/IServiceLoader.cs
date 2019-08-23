using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IServiceLoader
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureProvider(IServiceProvider provider);
    }
}
