
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI
{
    [Flags]
    public enum ElementState : int
    {
        None = 0,
        Hovered = 1,
        Focused = 4,
        Focus = 2,
    }
}
