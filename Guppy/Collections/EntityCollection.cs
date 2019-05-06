using Guppy.Factories;
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
            // Add the new entity to the current collection
            this.Add(entity);

            // Trigger the created event
            this.Created?.Invoke(this, entity);

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

        /// <summary>
        /// Create a new instance of an entity and adds it to
        /// the current collection's scene.
        /// 
        /// This entity required a custom degined guid
        /// </summary>
        /// <param name="entityHandle"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity Create(String entityHandle, Guid id, params Object[] args)
        {
            // Create the new entity
            var entity = _entityFactory.Create(entityHandle, id, args);
            // Add the new entity to the current collection
            this.Add(entity);

            // return the new entity
            return entity;
        }
        #endregion

        #region Collection Methods
        public override void Add(Entity item)
        {
            base.Add(item);

            // Create a new entry in the entity layer table for the new item
            _entityLayerTable.Add(item, null);
            item.OnLayerDepthChanged += this.HandleLayerDepthChanged;

            // When a new entity gets added, we must initialize it
            item.TryBoot();
            item.TryPreInitialize();
            item.TryInitialize();
            item.TryPostInitialize();

            // Update the entities initial layer
            this.UpdateEntityLayer(item);
        }

        public override bool Remove(Entity item)
        {
            if(base.Remove(item))
            {
                // Remove the entity from its old layer (if one exists)
                _entityLayerTable[item]?.entities.Remove(item);

                // Remove the item from the entity layer table
                _entityLayerTable.Remove(item);
                item.OnLayerDepthChanged -= this.HandleLayerDepthChanged;

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
        private void HandleLayerDepthChanged(object sender, Entity e)
        {
            this.UpdateEntityLayer(e);
        }
        #endregion
    }
}
