using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    [Flags]
    public enum ElementState
    {
        Defaut = 1,
        Hovered = 2,
        Focused = 4,
        Active = 8
    }
}
