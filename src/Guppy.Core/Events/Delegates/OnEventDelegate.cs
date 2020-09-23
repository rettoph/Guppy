using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Events.Delegates
{
    public delegate void OnEventDelegate<TSender, TArgs>(TSender sender, TArgs args);
}
