using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    [Flags]
    public enum ElementState
    {
        Default = 0,
        Hovered = 1,
        Pressed = 2,
        Focused = 4
    }
}
