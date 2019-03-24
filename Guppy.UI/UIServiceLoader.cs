using Guppy.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI
{
    /// <summary>
    /// Service loader to interface UI 
    /// functionality with core guppy
    /// </summary>
    public class UIServiceLoader : IServiceLoader
    {
        public void Boot(IServiceCollection services)
        {
            throw new NotImplementedException();
        }

        public void PreInitialize(IServiceProvider provider)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            throw new NotImplementedException();
        }
    }
}
