using Guppy.DependencyInjection;
using Guppy.Lists;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Lists
{
    public class ComponentList<TComponent> : ServiceList<TComponent>
        where TComponent : class, IComponent
    {
        #region Private Fields
        public IBaseContainer Parent { get; set; }
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnAdd += this.AddComponent;
            this.OnRemove += this.RemoveComponent;
        }

        protected override void Release()
        {
            base.Release();

            this.OnAdd -= this.AddComponent;
            this.OnRemove -= this.RemoveComponent;
        }
        #endregion

        #region Collection Methods
        private void AddComponent(TComponent item)
        {
            item.Container = this.Parent;
        }

        private void RemoveComponent(TComponent item)
        {
            item.Container = null;
        }
        #endregion
    }
}
