using Guppy.Collections;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Collections
{
    public sealed class ComponentCollection : ServiceCollection<IComponent>
    {
        #region Private Fields
        public IElement Parent { get; set; }
        #endregion

        #region Constructor
        public ComponentCollection()
        {
        }
        #endregion

        #region Collection Methods
        protected override void Add(IComponent item)
        {
            base.Add(item);

            item.Container = this.Parent;
        }

        protected override void Remove(IComponent item)
        {
            base.Remove(item);

            item.Container = null;
        }
        #endregion
    }
}
