using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Factories;
using Guppy.Interfaces;
using Guppy.Utilities.LayerDepths;

namespace Guppy.Collections
{
    public class LayerCollection : ConfigurableCollection<ILayer>
    {
        public LayerCollection(ConfigurableFactory<ILayer> factory, IServiceProvider provider) : base(factory, provider)
        {
        }

        #region Collection Methods
        public override bool Add(ILayer item)
        {
            if (this.Overlaps(item.Depth))
                return false;

            return base.Add(item);
        }
        #endregion

        #region Create Methods 
        public TLayer Create<TLayer>(Int32 index, Action<TLayer> setup = null, Action<TLayer> create = null)
            where TLayer : Layer
        {
            return this.Create<TLayer>(
                l =>
                {
                    l.Depth = index;

                    setup?.Invoke(l);
                }, create);
        }

        public TLayer Create<TLayer>(Int32 min, Int32 max, Action<TLayer> setup = null, Action<TLayer> create = null)
            where TLayer : Layer
        {
            return this.Create<TLayer>(
                l =>
                {
                    l.Depth = new Int32[] { min, max };

                    setup?.Invoke(l);
                }, create);
        }

        public TLayer Create<TLayer>(LayerDepth indices, Action<TLayer> setup = null, Action<TLayer> create = null)
            where TLayer : Layer
        {
            return this.Create<TLayer>(
                l =>
                {
                    l.Depth = indices;

                    setup?.Invoke(l);
                }, create);
        }
        #endregion

        #region Get Methods 
        public ILayer GetByDepth(Int32 depth)
        {
            return this.FirstOrDefault(l => l.Depth.Contains(depth));
        }

        private Boolean Overlaps(LayerDepth depth)
        {
            return this.FirstOrDefault(l => l.Depth.Overlap(depth)) != default(Layer);
        }
        #endregion
    }
}
