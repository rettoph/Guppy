using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using Guppy.Extensions;

namespace Guppy.Collections
{
    /// <summary>
    /// The main entity collection, this list will contain all entities
    /// within a single scene.
    /// </summary>
    public sealed class EntityCollection : ServiceCollection<Entity>
    {
        #region Private Fields
        private LayerCollection _layers;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _layers = provider.GetService<LayerCollection>();
        }
        #endregion

        #region Collection Methods
        protected override void Add(Entity item)
        {
            base.Add(item);

            item.OnLayerGroupChanged += this.HandleItemLayerGroupChanged;
            this.UpdateItemLayer(item);
        }

        protected override void Remove(Entity item)
        {
            base.Remove(item);

            item.OnLayerGroupChanged -= this.HandleItemLayerGroupChanged;
            item.Layer.Entities.TryRemove(item);
            item.Layer = null;
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
