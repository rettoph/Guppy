using Guppy.UI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IComponent : IElement
    {
        #region Attributes
        IElement Container { get; set; }
        Boolean Active { get; }
        Cursor.Button Buttons { get; }
        #endregion

        #region Events
        event EventHandler<Boolean> OnActiveChanged;
        event EventHandler<Cursor.Button> OnButtonPressed;
        event EventHandler<Cursor.Button> OnButtonReleased;
        #endregion
    }
}
