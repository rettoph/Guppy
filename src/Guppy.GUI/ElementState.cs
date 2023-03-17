
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
        Focused = 1,
        Hovered = 2,
    }
}
