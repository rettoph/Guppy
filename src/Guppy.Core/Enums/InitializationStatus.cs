using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Enums
{
    public enum InitializationStatus
    {
        NotCreated,
        Creating,
        NotReady,
        PreInitializing,
        Initializing,
        PostInitializing,
        Releasing,
        Disposing,
        Ready
    }
}
