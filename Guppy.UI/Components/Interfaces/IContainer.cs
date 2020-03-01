using Guppy.Collections;
using Guppy.UI.Collections;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components.Interfaces
{
    public interface IContainer<TElement> : IBaseElement
        where TElement : IElement
    {
        /// <summary>
        /// List of all children within the current container.
        /// </summary>
        ElementCollection<TElement> Children { get; }
    }
}
