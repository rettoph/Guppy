using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Interfaces
{
    public interface IButton : IComponent
    {
        event EventHandler OnClicked;
    }
}
