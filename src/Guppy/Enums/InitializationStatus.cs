using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Enums
{
    public enum InitializationStatus
    {
        NotReady,
        PreInitializing,
        Initializing,
        PostInitializing,
        Disposing,
        Ready
    }
}
