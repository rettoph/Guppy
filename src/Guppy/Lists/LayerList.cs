using Guppy.DependencyInjection;
using Guppy.Interfaces;
using Guppy.Lists.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Lists
{
    public class LayerList : OrderableList<ILayer>
    {
        #region Lifecycle Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.CanAdd += this.CanAddLayer;
        }

        protected override void Release()
        {
            base.Release();

            this.releaseChildren = true;

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
        private bool CanAddLayer(IServiceList<ILayer> list, ILayer item)
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
        public ILayer GetByGroup(Int32 group)
            => this.FirstOrDefault(l => l.Group.Contains(group));

        public T GetByGroup<T>(Int32 group)
            where T : class, ILayer
                => this.GetByGroup(group) as T;
        #endregion
    }
}
