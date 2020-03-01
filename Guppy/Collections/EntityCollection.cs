using Guppy.Factories;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public sealed class EntityCollection : ConfigurableCollection<IEntity>
    {
        #region Private Fields
        /// <summary>
        /// Contains a collection list of all entities and the layer
        /// they are currently residing in.
        /// 
        /// This is so the EntityColelction knows what layer the entity needs
        /// to be remoed from when the changed:layer event is invoked.
        /// </summary>
        private Dictionary<IEntity, ILayer> _cachedLayers;

        private LayerCollection _layers;
        #endregion

        #region Constructor
        public EntityCollection(ConfigurableFactory<IEntity> factory, LayerCollection layers, IServiceProvider provider) : base(factory, provider)
        {
            _cachedLayers = new Dictionary<IEntity, ILayer>();
            _layers = layers;
        }
        #endregion

        #region Collection Methods
        public override bool Add(IEntity item)
        {
            if (base.Add(item))
            {
                item.OnLayerDepthChanged += this.HandleItemLayerDepthChanged;

                _cachedLayers.Add(item, null);
                this.AddToLayer(item);

                return true;
            }

            return false;
        }

        public override bool Remove(IEntity item)
        {
            if (base.Remove(item))
            {
                item.OnLayerDepthChanged -= this.HandleItemLayerDepthChanged;

                this.RemoveFromLayer(item);
                _cachedLayers.Remove(item);

                return true;
            }

            return false;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Remove the item from its old layer, if there was any
        /// </summary>
        /// <param name="item"></param>
        private void RemoveFromLayer(IEntity item)
        {
            if (_cachedLayers[item] != null)
                _cachedLayers[item]?.Entities.Remove(item);
        }

        /// <summary>
        /// Add the item to its current layer
        /// </summary>
        /// <param name="item"></param>
        private void AddToLayer(IEntity item)
        {
            // First remove the entity to whatever layer it was on, if any
            this.RemoveFromLayer(item);

            var layer = _layers.GetByDepth(item.LayerDepth);

            layer?.Entities.Add(item);
            // Store the current layer
            _cachedLayers[item] = layer;
        }
        #endregion

        #region Event Handlers
        private void HandleItemLayerDepthChanged(object sender, Int32 arg)
        {
            this.AddToLayer(sender as Entity);
        }
        #endregion
    }
}
