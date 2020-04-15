using Guppy.LayerGroups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xxHashSharp;

namespace Guppy.Collections
{
    public sealed class LayerCollection : OrderableCollection<Layer>
    {
        #region Collection Methods
        protected override void Add(Layer item)
        {
            foreach (Layer layer in this)
                if (item.Group.Overlap(layer.Group))
                    throw new Exception("Unable to add Layer to collecion, Group overlap detected.");

            base.Add(item);
        }
        #endregion

        #region Helper Methods
        public Layer GetByGroup(Int32 group)
        {
            return this.FirstOrDefault(l => l.Group.Contains(group));
        }
        public T GetByGroup<T>(Int32 group)
            where T : Layer
        {
            return this.FirstOrDefault(l => l.Group.Contains(group)) as T;
        }
        #endregion
    }
}
