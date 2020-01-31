using Guppy.Collections;
using Guppy.Extensions.Collection;
using Guppy.UI.Entities.UI.Interfaces;
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

namespace Guppy.UI.Entities.UI
{
    /// <summary>
    /// Basic implementation of the IContainer interface designed to hold
    /// any type extending Element
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class Container<TElement> : ProtectedContainer<TElement>, IContainer<TElement>
        where TElement : Element
    {
        #region Protected Fields
        public IReadOnlyCollection<TElement> Children => this.children;
        #endregion

        #region IContainer Implementation
        public TElement Add(TElement child)
        {
            return this.add(child);
        }

        public T Add<T>(string handle, Action<T> setup = null, Action<T> create = null) 
            where T : TElement
        {
            return this.add<T>(handle, setup, create);
        }

        public T Add<T>(Action<T> setup = null, Action<T> create = null) 
            where T : TElement
        {
            return this.add<T>(setup, create);
        }

        public void Remove(TElement child)
        {
            this.remove(child);
        }
        #endregion
    }

    public class Container : Container<Element> {

    }
}
