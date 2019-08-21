using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Utilities.Factories;

namespace Guppy.Collections
{
    public class LayerCollection : FrameableCollection<Layer>
    {
        private PooledFactory<Layer> _factory;

        public LayerCollection(PooledFactory<Layer> factory, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
        }

        #region Create Methods
        public TLayer Create<TLayer>(Action<TLayer> setup = null)
            where TLayer : Layer
        {
            var layer = _factory.Pull<TLayer>(setup);
            this.Add(layer);
            return layer;
        }
        #endregion
    }
}
