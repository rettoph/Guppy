using Guppy.Extensions;
using Guppy.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using Pong.Library.Layers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pong.Library.ServiceLoaders
{
    public class PongGameServiceLoader : IServiceLoader
    {
        public void ConfigureServiceCollection(IServiceCollection services)
        {
            services.AddLayer<SimpleLayer>();
        }

        public void Boot(IServiceProvider provider)
        {

        }

        public void PreInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void Initialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }

        public void PostInitialize(IServiceProvider provider)
        {
            // throw new NotImplementedException();
        }
    }
}
