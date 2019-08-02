using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Service loaders are the heart of Guppy, automatically loaded
    /// by the GuppyLoader and called on GuppyLoader.Initialize.
    /// 
    /// Service loaders should be used to interface any and all
    /// custom functionality into the Guppy Game Engine.
    /// </summary>
    public interface IServiceLoader
    {
        void Boot(IServiceCollection services);
        void PreInitialize(IServiceProvider provider);
        void Initialize(IServiceProvider provider);
        void PostInitialize(IServiceProvider provider);
    }
}
