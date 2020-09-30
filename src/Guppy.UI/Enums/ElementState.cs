using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    [Flags]
    public enum ElementState
    {
        None = 0,
        Hovered = 1,
        Pressed = 2,
        Active = 4
    }
}
