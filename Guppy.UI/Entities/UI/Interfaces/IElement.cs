using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Entities.UI.Interfaces
{
    public interface IElement
    {
        #region Attributes
        /// <summary>
        /// The element bounds.
        /// </summary>
        ElementBounds Bounds { get; }

        /// <summary>
        /// The current hover state.
        /// </summary>
        Boolean Hovered { get; }

        /// <summary>
        /// The current active state.
        /// </summary>
        Boolean Active { get; }

        /// <summary>
        /// The pointer buttons currently pressed on this element.
        /// </summary>
        Pointer.Button Buttons { get; }
        #endregion

        #region Events
        event EventHandler<Boolean> OnHoveredChanged;
        event EventHandler<Boolean> OnActiveChanged;
        event EventHandler<Pointer.Button> OnButtonPressed;
        event EventHandler<Pointer.Button> OnButtonReleased;
        #endregion

        #region Methods
        /// <summary>
        /// Return the current element's container's bounds.
        /// 
        /// This should be in pixels & world coordinates.
        /// </summary>
        /// <returns></returns>
        Rectangle GetContainerBounds();

        /// <summary>
        /// Attempt to clean the current element.
        /// This will also call mark all internal
        /// children as dirty.
        /// </summary>
        /// <param name="force"></param>
        void TryClean(Boolean force = false);
        #endregion
    }
}
