using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Enums
{
    public enum ServiceStatus
    {
        NotCreated,
        PreCreating,
        Creating,
        PostCreating,
        NotReady,
        PreInitializing,
        Initializing,
        PostInitializing,
        Ready,
        PreReleasing,
        Releasing,
        PostReleasing,
        PreDisposing,
        Disposing,
        PostDisposing,
    }
}
