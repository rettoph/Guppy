using Guppy.DependencyInjection;
using Guppy.Lists.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Lists
{
    public class LayerList : OrderableList<Layer>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.CanAdd += this.CanAddLayer;
        }

        protected override void Release()
        {
            base.Release();

            this.CanAdd -= this.CanAddLayer;
        }
        #endregion

        #region Collection Methods
        /// <summary>
        /// Throw an exception when a later is added to the
        /// collection.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool CanAddLayer(IServiceList<Layer> list, Layer item)
        {
            foreach (Layer layer in this)
                if (item.Id == layer.Id)
                    return false;
                else if (item.Group.Overlap(layer.Group))
                    throw new Exception("Unable to add Layer to collection, Group overlap detected.");

            return true;
        }
        #endregion

        #region Helper Methods
        public Layer GetByGroup(Int32 group)
            => this.First(l => l.Group.Contains(group));

        public T GetByGroup<T>(Int32 group)
            where T : Layer
                => this.GetByGroup(group) as T;
        #endregion
    }
}
