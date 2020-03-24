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

        #region Factory Methods
        public T Create<T>(UInt32 id, LayerGroup group, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return (T)this.Create(id, (p, i) =>
            {
                i.Group = group; // Update the group.
                setup?.Invoke(p, (T)i);
            });
        }
        public T Create<T>(String handle, LayerGroup group, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(xxHash.CalculateHash(Encoding.UTF8.GetBytes(handle)), group, setup);
        }
        public T Create<T>(Type serviceType, LayerGroup group, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(serviceType.FullName, group, setup);
        }

        public T Create<T>(UInt32 id, Int32 value, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(id, new SingleLayerGroup(value), setup);
        }
        public T Create<T>(String handle, Int32 value, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(handle, new SingleLayerGroup(value), setup);
        }
        public T Create<T>(Type serviceType, Int32 value, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(serviceType, new SingleLayerGroup(value), setup);
        }

        public T Create<T>(UInt32 id, Int32 min, Int32 max, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(id, new RangeLayerGroup(min, max), setup);
        }
        public T Create<T>(String handle, Int32 min, Int32 max, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(handle, new RangeLayerGroup(min, max), setup);
        }
        public T Create<T>(Type serviceType, Int32 min, Int32 max, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(serviceType, new RangeLayerGroup(min, max), setup);
        }

        public T Create<T>(Int32 value, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(typeof(T), new SingleLayerGroup(value), setup);
        }
        public T Create<T>(Int32 min, Int32 max, Action<ServiceProvider, T> setup = null)
            where T : Layer
        {
            return this.Create<T>(typeof(T), new RangeLayerGroup(min, max), setup);
        }
        #endregion
    }
}
