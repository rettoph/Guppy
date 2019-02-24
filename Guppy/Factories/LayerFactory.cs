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
            return this.Create(provider, new LayerConfiguration());
        }

        public TLayer Create(IServiceProvider provider, LayerConfiguration configuration)
        {
            // Create a new layer instance
            TLayer layer = ActivatorUtilities.CreateInstance(provider, this.targetType, configuration) as TLayer;

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
