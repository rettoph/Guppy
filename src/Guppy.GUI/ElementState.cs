
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    [Flags]
    public enum ElementState
    {
        None = 1,
        Hovered = 2,
        Focused = 4,
        Any = None | Hovered | Focused
    }
}
