using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Collections;
using Guppy.UI.Components.Interfaces;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.UI.Components
{
    /// <summary>
    /// Basic implementation of the IContainer interface designed to hold
    /// any type extending Element
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class Container<TElement> : ProtectedContainer<TElement>, IContainer<TElement>
        where TElement : IElement
    {
        #region Public Fields
        public ElementCollection<TElement> Children { get => this.children; }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);
        }
        #endregion
    }

    public class Container : Container<Element> {

    }
}
