using Guppy.Interfaces;
using Guppy.UI.Entities;
using Guppy.UI.Enums;
using Guppy.UI.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components.Interfaces
{
    public interface IElement : IConfigurable, IBaseElement
    {
        #region Attributes
        /// <summary>
        /// The element bounds.
        /// </summary>
        ElementBounds Bounds { get; }

        /// <summary>
        /// The current active state.
        /// </summary>
        Boolean Active { get; }

        /// <summary>
        /// The pointer buttons currently pressed on this element.
        /// </summary>
        Pointer.Button Buttons { get; }
        IBaseElement Container { get; set; }
        #endregion

        #region Events
        event EventHandler<Boolean> OnHoveredChanged;
        event EventHandler<Boolean> OnActiveChanged;
        event EventHandler<Pointer.Button> OnButtonPressed;
        event EventHandler<Pointer.Button> OnButtonReleased;
        #endregion
    }
}
