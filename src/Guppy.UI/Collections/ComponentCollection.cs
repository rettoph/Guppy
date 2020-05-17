using Guppy.Collections;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Collections
{
    public class ComponentCollection<TComponent> : ServiceCollection<TComponent>
        where TComponent : IComponent
    {
        #region Private Fields
        public IBaseContainer Parent { get; set; }
        #endregion

        #region Constructor
        public ComponentCollection()
        {
        }
        #endregion

        #region Collection Methods
        protected override void Add(TComponent item)
        {
            base.Add(item);

            item.Container = this.Parent;
        }

        protected override void Remove(TComponent item)
        {
            base.Remove(item);

            item.Container = null;
        }
        #endregion
    }
}
