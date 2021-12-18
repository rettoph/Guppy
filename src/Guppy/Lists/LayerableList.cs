using Guppy.Lists.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.Interfaces;
using Guppy.EntityComponent.DependencyInjection;

namespace Guppy.Lists
{
    public class LayerableList : OrderableList<ILayerable>
    {
        #region Private Fields
        private LayerList _layers;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.releaseChildren = true;

            _layers = provider.GetService<LayerList>();

            this.OnAdd += this.AddLayerable;
            this.OnRemove += this.RemoveLayerable;
        }

        protected override void Release()
        {
            base.Release();

            this.OnAdd -= this.AddLayerable;
            this.OnRemove -= this.RemoveLayerable;
        }
        #endregion

        #region Collection Methods
        /// <summary>
        /// When a layerable is added, we must auto update the layer.
        /// </summary>
        /// <param name="item"></param>
        private void AddLayerable(ILayerable item)
        {
            item.OnLayerGroupChanged += this.HandleItemLayerGroupChanged;
            this.UpdateItemLayer(item);
        }

        /// <summary>
        /// When an entity is removed we must remove it from the layer
        /// and clear all relevant layer data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="item"></param>
        private void RemoveLayerable(ILayerable item)
        {
            item.OnLayerGroupChanged -= this.HandleItemLayerGroupChanged;
            item.Layer?.Items.TryRemove(item);
            item.Layer = null;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Automatically update a recieved entity's layer.
        /// </summary>
        /// <param name="item"></param>
        private void UpdateItemLayer(ILayerable item)
        {
            // Remove from old layer...
            item.Layer?.Items.TryRemove(item);

            // Add into the new layer...
            item.Layer = _layers.GetByGroup(item.LayerGroup);
            item.Layer?.Items.TryAdd(item);
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
            => this.UpdateItemLayer(sender as ILayerable);
        #endregion
    }
}
