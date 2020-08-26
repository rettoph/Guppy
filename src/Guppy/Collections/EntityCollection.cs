using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using Guppy.Extensions;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Collections
{
    /// <summary>
    /// The main entity collection, this list will contain all entities
    /// within a single scene.
    /// </summary>
    public sealed class EntityCollection : FactoryCollection<Entity>
    {
        #region Private Fields
        private LayerCollection _layers;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _layers = provider.GetService<LayerCollection>();

            this.OnAdd += this.AddEntity;
            this.OnRemove += this.RemoveEntity;
        }

        protected override void Dispose()
        {
            base.Dispose();

            this.OnAdd -= this.AddEntity;
            this.OnRemove -= this.RemoveEntity;
        }
        #endregion

        #region Collection Methods
        /// <summary>
        /// When an entity is added, we must auto update the layer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="entity"></param>
        private void AddEntity(Entity entity)
        {
            entity.OnLayerGroupChanged += this.HandleItemLayerGroupChanged;
            this.UpdateItemLayer(entity);
        }

        /// <summary>
        /// When an entity is removed we must remove it from the layer
        /// and clear all relevant layer data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="entity"></param>
        private void RemoveEntity(Entity entity)
        {
            entity.OnLayerGroupChanged -= this.HandleItemLayerGroupChanged;
            entity.Layer.Entities.TryRemove(entity);
            entity.Layer = null;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Automatically update a recieved entity's layer.
        /// </summary>
        /// <param name="item"></param>
        private void UpdateItemLayer(Entity item)
        {
            // Add into the new layer...
            _layers.GetByGroup(item.LayerGroup).Entities.TryAdd(item);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Automatically update an entities Layer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleItemLayerGroupChanged(object sender, int e)
        {
            this.UpdateItemLayer(sender as Entity);
        }
        #endregion
    }
}
