using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI.Interfaces
{
    /// <summary>
    /// A container is a UI element that can publicly add
    /// multiple children.
    /// </summary>
    public interface IContainer<TElement>
        where TElement : BaseElement
    {
        /// <summary>
        /// Add a pre existing element into the current container.
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        TElement Add(TElement child);

        /// <summary>
        /// Create a new element & add it to the current container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handle"></param>
        /// <param name="setup"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        T Add<T>(String handle, Action<T> setup = null, Action<T> create = null)
            where T : TElement;

        /// <summary>
        /// Create a new element & add it to the current container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setup"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        T Add<T>(Action<T> setup = null, Action<T> create = null)
            where T : TElement;

        /// <summary>
        /// Remove the given element from the current container.
        /// </summary>
        /// <param name="child"></param>
        void Remove(TElement child);
    }
}
