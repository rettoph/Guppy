using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Factories;
using Microsoft.Extensions.Logging;

namespace Guppy.Collections
{
    public sealed class LayerCollection : FrameableCollection<Layer>
    {
        private InitializableFactory<Layer> _factory;

        public LayerCollection(InitializableFactory<Layer> factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
        }

        #region Create Methods
        public TLayer Create<TLayer>(Action<TLayer> setup = null)
            where TLayer : Layer
        {
            var layer = _factory.Build<TLayer>(setup);
            this.Add(layer);
            return layer;
        }
        #endregion
    }
}
