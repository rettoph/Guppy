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
        NotInitialized,
        PreInitializing,
        Initializing,
        PostInitializing,
        PostInitialized,
        Ready,
        PreReleasing,
        Releasing,
        PostReleasing,
        PreDisposing,
        Disposing,
        PostDisposing,
    }
}
