using Guppy.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Guppy.Collections
{
    public class LayerCollection : ReusableCollection<Layer>
    {
        #region Private Fields
        private IServiceProvider _provider;
        #endregion

        #region Constructors
        public LayerCollection(IServiceProvider provider) : base(provider)
        {
            _provider = provider;
        }
        #endregion

        #region Collection Methods
        public override bool Add(Layer item)
        {
            this.ValidateLayerDepth(item);

            if (base.Add(item))
            { // When a new layer gets added...
                item.Events.AddDelegate<UInt16>("changed:depth", this.HandleItemDepthChanged);

                return true;
            }

            return false;
        }

        public override bool Remove(Layer item)
        {
            if (base.Add(item))
            { // When a layer gets removed...
                item.Events.RemoveDelegate<UInt16>("changed:depth", this.HandleItemDepthChanged);

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        public TLayer Build<TLayer>(UInt16 depth, Action<TLayer> setup = null)
            where TLayer : Layer
        {
            // Create new layer instance...
            var layer = _provider.GetPooledService<TLayer>(l =>
            {
                // Update the layers depth...
                l.SetDepth(depth);

                // Call the custom setup function...
                setup?.Invoke(l);
            });

            // Automatically add the new layer to the current collection...
            this.Add(layer);

            // Return the layer instance...
            return layer;
        }

        /// <summary>
        /// Verify that no other layers within the current collection
        /// contain a depth of the requested value.
        /// </summary>
        /// <param name="blacklist"></param>
        private void ValidateLayerDepth(Layer blacklist)
        {
            if(this.FirstOrDefault(l => l.Depth == blacklist.Depth && l != blacklist) != default(Layer))
                throw new Exception("Invalid layer depth. Another layer with this depth already resides within the collection.");
        }
        #endregion

        #region Event Handlers
        private void HandleItemDepthChanged(object sender, UInt16 arg)
        {
            this.ValidateLayerDepth(sender as Layer);
        }
        #endregion
    }
}
