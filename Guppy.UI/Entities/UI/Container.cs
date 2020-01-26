using Guppy.UI.Entities.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI
{
    public class Container : StyleElement, IContainer
    {
        #region IContainer Implementation
        /// <inheritdoc />
        public Element Add(Element child)
        {
            return this.add(child);
        }

        /// <inheritdoc />
        public T Add<T>(String handle, Action<T> setup = null, Action<T> create = null) where T : Element
        {
            return this.add<T>(handle, setup, create);
        }

        /// <inheritdoc />
        public T Add<T>(Action<T> setup = null, Action<T> create = null) where T : Element
        {
            return this.add<T>(setup, create);
        }

        /// <inheritdoc />
        public void Remove(Element child)
        {
            this.remove(child);
        }
        #endregion
    }
}
