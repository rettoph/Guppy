using Guppy.Factories;
using Guppy.Utilities.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public sealed class EntityCollection : OrderableCollection<Entity>
    {
        #region Private Fields
        /// <summary>
        /// Contains a collection list of all entities and the layer
        /// they are currently residing in.
        /// 
        /// This is so the EntityColelction knows what layer the entity needs
        /// to be remoed from when the changed:layer event is invoked.
        /// </summary>
        private Dictionary<Entity, Layer> _cachedLayers;

        private EntityFactory _factory;
        #endregion

        #region Constructor
        public EntityCollection(EntityFactory factory, LayerCollection layers, IServiceProvider provider) : base(provider)
        {
            _factory = factory;
            _cachedLayers = new Dictionary<Entity, Layer>();
        }
        #endregion

        #region Lifecycle Methods
        public override void Dispose()
        {
            base.Dispose();

            _cachedLayers.Clear();
        }
        #endregion

        #region Collection Methods
        public override bool Add(Entity item)
        {
            if(base.Add(item))
            {
                item.Events.TryAdd<Layer>("changed:layer", this.HandleItemLayerChanged);

                _cachedLayers.Add(item, null);
                this.AddToLayer(item);

                return true;
            }

            return false;
        }

        public override bool Remove(Entity item)
        {
            if (base.Remove(item))
            {
                item.Events.TryRemove<Layer>("changed:layer", this.HandleItemLayerChanged);

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
        private void RemoveFromLayer(Entity item)
        {
            if (_cachedLayers[item] != null)
                _cachedLayers[item].entities.Remove(item);
        }

        /// <summary>
        /// Add the item to its current layer
        /// </summary>
        /// <param name="item"></param>
        private void AddToLayer(Entity item)
        {
            // First remove the entity to whatever layer it was on, if any
            this.RemoveFromLayer(item);

            if(item.Layer != null)
                item.Layer.entities.Add(item);

            // Store the current layer
            _cachedLayers[item] = item.Layer;
        }
        #endregion

        #region Create Method
        public TEntity Create<TEntity>(String handle, Action<TEntity> setup = null)
            where TEntity : Entity
        {
            var entity = _factory.Build<TEntity>(handle, setup);
            this.Add(entity);
            return entity;
        }
        public Entity Create(String handle, Action<Entity> setup = null)
        {
            return this.Create<Entity>(handle, setup);
        }
        #endregion

        #region Event Handlers
        private void HandleItemLayerChanged(object sender, Layer arg)
        {
            this.AddToLayer(sender as Entity);
        }
        #endregion
    }
}
