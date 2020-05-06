using Guppy.UI.Entities;
using Guppy.UI.Utilities.Backgrounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IComponent : IElement
    {
        #region Attributes
        IBaseContainer Container { get; set; }
        Boolean Active { get; }
        Cursor.Button Buttons { get; }
        Background Background { get; set; }
        #endregion

        #region Events
        event EventHandler<Boolean> OnActiveChanged;
        event EventHandler<Cursor.Button> OnButtonPressed;
        event EventHandler<Cursor.Button> OnButtonReleased;
        event EventHandler<Background> OnBackgroundChanged;
        #endregion
    }
}
