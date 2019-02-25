using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Stores a collection of all entities within a scene.
    /// </summary>
    public class EntityCollection : LivingObjectCollection<Entity>
    {
        #region Private Fields
        private LayerCollection _layers;
        private Dictionary<Entity, Layer> _entityLayerTable;
        #endregion

        #region Constructors
        public EntityCollection(LayerCollection layers)
        {
            _layers = layers;
            _entityLayerTable = new Dictionary<Entity, Layer>();
        }
        #endregion

        #region Collection Methods
        public override void Add(Entity item)
        {
            base.Add(item);

            // Create a new entry in the entity layer table for the new item
            _entityLayerTable.Add(item, null);
            item.OnLayerDepthChanged += this.HandleLayerDepthChanged;

            // Update the entities initial layer
            this.UpdateEntityLayer(item);
        }

        public override bool Remove(Entity item)
        {
            if(base.Remove(item))
            {
                // Remove the item from the entity layer table
                _entityLayerTable.Remove(item);
                item.OnLayerDepthChanged -= this.HandleLayerDepthChanged;

                // Remove the entity from its old layer (if one exists)
                _entityLayerTable[item]?.entities.Remove(item);

                return true;
            }

            return false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Remove an entity from its old tracked layer, and add it
        /// to its new layer
        /// </summary>
        /// <param name="item"></param>
        private void UpdateEntityLayer(Entity item)
        {
            // Remove the entity from its old layer (if one exists)
            _entityLayerTable[item]?.entities.Remove(item);

            // Add the entity to its new layer
            var layer = _layers.GetLayer(item.LayerDepth);
            layer.entities.Add(item);

            // Update the stored layer value
            _entityLayerTable[item] = layer;
        }
        #endregion

        #region Event Handlers
        private void HandleLayerDepthChanged(object sender, Entity e)
        {
            this.UpdateEntityLayer(e);
        }
        #endregion
    }
}
