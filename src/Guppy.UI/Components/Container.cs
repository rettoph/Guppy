using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.UI.Collections;
using Guppy.UI.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components
{
    public class Container<TComponent> : ProtectedContainer<TComponent>, IContainer<TComponent>
        where TComponent : IComponent
    {
        #region Public Attributes
        public ComponentCollection<TComponent> Children => this.children;
        #endregion
    }
}
