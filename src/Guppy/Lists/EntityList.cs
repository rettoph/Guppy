using Guppy.DependencyInjection;
using Guppy.Lists.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;
using System.Linq;
using Guppy.Interfaces;

namespace Guppy.Lists
{
    public class EntityList : OrderableList<IEntity>
    {
        #region Private Fields
        private LayerList _layers;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.releasedChildren = true;

            _layers = provider.GetService<LayerList>();

            this.OnAdd += this.AddEntity;
            this.OnRemove += this.RemoveEntity;
        }

        protected override void Release()
        {
            base.Release();

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
        private void AddEntity(IEntity entity)
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
        private void RemoveEntity(IEntity entity)
        {
            entity.OnLayerGroupChanged -= this.HandleItemLayerGroupChanged;
            entity.Layer?.Entities.TryRemove(entity);
            entity.Layer = null;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Automatically update a recieved entity's layer.
        /// </summary>
        /// <param name="item"></param>
        private void UpdateItemLayer(IEntity item)
        {
            // Remove from old layer...
            item.Layer?.Entities.TryRemove(item);

            // Add into the new layer...
            item.Layer = _layers.GetByGroup(item.LayerGroup);
            item.Layer?.Entities.TryAdd(item);
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Automatically update an entities Layer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="old"></param>
        /// <param name="value"></param>
        private void HandleItemLayerGroupChanged(object sender, Int32 old, Int32 value)
            => this.UpdateItemLayer(sender as IEntity);
        #endregion
    }
}
