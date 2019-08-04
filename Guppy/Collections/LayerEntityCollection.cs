using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    /// <summary>
    /// Represents a collection of entities residing
    /// within a layer.
    /// </summary>
    public class LayerEntityCollection : ReusableCollection<Entity>
    {
        public LayerEntityCollection(IServiceProvider provider) : base(provider)
        {
        }

        #region Collection Methods
        public override bool Add(Entity item)
        {
            if (base.Add(item))
            { // When a new entity gets added...
                item.Events.AddDelegate<UInt16>("changed:layer-depth", this.HandleItemLayerDepthChanged);

                return true;
            }

            return false;
        }

        public override bool Remove(Entity item)
        {
            if (base.Remove(item))
            { // When a entity gets removed...
                item.Events.RemoveDelegate<UInt16>("changed:layer-depth", this.HandleItemLayerDepthChanged);

                return true;
            }

            return false;
        }
        #endregion

        #region Event Handlers
        private void HandleItemLayerDepthChanged(object sender, UInt16 arg)
        {
            this.Remove(sender as Entity);
        }
        #endregion
    }
}
