using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Service providers are custom objects used to help setup a game
    /// object externally. They can register services, interact with loaders,
    /// and more all within their own self-contained class.
    /// </summary>
    public interface IServiceLoader
    {
        void ConfigureServiceCollection(IServiceCollection services);
        void Boot(IServiceProvider provider);
        void PreInitialize(IServiceProvider provider);
        void Initialize(IServiceProvider provider);
        void PostInitialize(IServiceProvider provider);
    }
}
