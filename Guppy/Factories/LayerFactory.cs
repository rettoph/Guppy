using Guppy.Collections;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.System;

namespace Guppy.Factories
{
    public class LayerFactory<TLayer> : InitializableFactory<TLayer>
        where TLayer : Layer
    {
        public override TLayer Create(IServiceProvider provider, params Object[] args)
        {
            return base.Create(provider, args);
        }

        public TLayer Create(IServiceProvider provider, LayerConfiguration configuration)
        {
            return base.Create(provider, configuration);
        }

        public TLayer Create(IServiceProvider provider, LayerConfiguration configuration, params Object[] args)
        {
            // Add the configuration to the args array
            args = args.AddItems(configuration);

            // Create the layer instance...
            return base.Create(provider, args);
        }

        public static LayerFactory<T> BuildFactory<T>()
            where T : Layer
        {
            return new LayerFactory<T>();
        }
    }
}
