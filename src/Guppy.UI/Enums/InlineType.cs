using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Enums
{
    [Flags]
    public enum InlineType
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
        Both = 1 | 2
    }
}
