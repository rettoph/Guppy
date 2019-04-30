using Guppy.Collections;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Factories
{
    public class LayerFactory<TLayer> : Factory<TLayer>
        where TLayer : Layer
    {
        public override TLayer Create(IServiceProvider provider)
        {
            return this.CreateCustom(provider);
        }

        public TLayer Create(IServiceProvider provider, LayerConfiguration configuration)
        {
            return this.CreateCustom(provider, configuration);
        }

        public override TLayer CreateCustom(IServiceProvider provider, params object[] args)
        {
            return this.CreateCustom(provider, new LayerConfiguration(), args);
        }
        public TLayer CreateCustom(IServiceProvider provider, LayerConfiguration configuration, params object[] args)
        {
            List<Object> lArgs = new List<Object>();
            lArgs.Add(configuration);
            lArgs.AddRange(args);

            // Create a new layer instance
            TLayer layer = ActivatorUtilities.CreateInstance(provider, this.targetType, lArgs.ToArray()) as TLayer;

            // Auto add the layer to the scope's layer collection
            var layers = provider.GetRequiredService<LayerCollection>();
            layers.Add(layer);

            // Return the new layer
            return layer;
        }

        public static LayerFactory<T> BuildFactory<T>()
            where T : Layer
        {
            return new LayerFactory<T>();
        }
    }
}
