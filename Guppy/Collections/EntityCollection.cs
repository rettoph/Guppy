using Guppy.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Stores a collection of all entities within a scene.
    /// </summary>
    public class EntityCollection : ZFrameableCollection<Entity>
    {
        #region Private Fields
        private LayerCollection _layers;
        private EntityFactory _entityFactory;
        private Dictionary<Entity, Layer> _entityLayerTable;
        #endregion

        #region Events
        public event EventHandler<Entity> Created;
        #endregion

        #region Constructors
        public EntityCollection(EntityFactory entityFactory, LayerCollection layers)
        {
            _layers = layers;
            _entityFactory = entityFactory;
            _entityLayerTable = new Dictionary<Entity, Layer>();
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Create a new instance of an entity and adds it to
        /// the current collection's scene 
        /// </summary>
        /// <param name="entityHandle"></param>
        /// <returns></returns>
        public Entity Create(String entityHandle, params Object[] args)
        {
            // Create the new entity
            var entity = _entityFactory.Create(entityHandle, args);
            // Trigger the created event
            this.Created?.Invoke(this, entity);

            // Add the new entity to the current collection
            this.Add(entity);

            // return the new entity
            return entity;
        }
        public TEntity Create<TEntity>(String entityHandle, params Object[] args)
            where TEntity : Entity
        {
            return this.Create(entityHandle, args) as TEntity;
        }
        public Entity Create(String entityHandle, UInt16 layerDepth, params Object[] args)
        {
            // Create the new entity
            var entity = this.Create(entityHandle, args);
            entity.SetLayerDepth(layerDepth);

            return entity;
        }
        public TEntity Create<TEntity>(String entityHandle, UInt16 layerDepth, params Object[] args)
            where TEntity : Entity
        {
            return this.Create(entityHandle, layerDepth, args) as TEntity;
        }
        #endregion

        #region Collection Methods
        public override void Add(Entity item)
        {
            // Create a new entry in the entity layer table for the new item
            _entityLayerTable.Add(item, null);
            item.Events.AddHandler("changed:layer-depth", this.HandleLayerDepthChanged);

            // Update the entities initial layer
            this.UpdateEntityLayer(item);

            // Add the item to the collection
            base.Add(item);
        }

        public override bool Remove(Entity item)
        {
            if(base.Remove(item))
            {
                // Remove the entity from its old layer (if one exists)
                _entityLayerTable[item]?.entities.Remove(item);

                // Remove the item from the entity layer table
                _entityLayerTable.Remove(item);
                item.Events.RemoveHandler("changed:layer-depth", this.HandleLayerDepthChanged);

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
            layer?.entities.Add(item);

            // Update the stored layer value
            _entityLayerTable[item] = layer;
        }
        #endregion

        #region Event Handlers
        private void HandleLayerDepthChanged(Object param)
        {
            this.UpdateEntityLayer(param as Entity);
        }
        #endregion
    }
}
