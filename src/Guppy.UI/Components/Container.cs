using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.UI.Lists;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class Container<TComponent> : ProtectedContainer<TComponent>, IContainer<TComponent>
        where TComponent : class, IComponent
    {
        #region Public Attributes
        public ComponentList<TComponent> Children => this.children;
        #endregion
    }
}
