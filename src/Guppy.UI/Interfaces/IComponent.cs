using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IComponent : IElement
    {
        #region Attributes
        IElement Container { get; set; }
        #endregion

        #region Events
        event EventHandler<Boolean> OnHoveredChanged;
        event EventHandler<Boolean> OnActiveChanged;
        #endregion
    }
}
