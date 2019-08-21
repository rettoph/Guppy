using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Enums
{
    public enum InitializationStatus
    {
        NotInitialized,
        PreInitializing,
        Initializing,
        PostInitializing,
        Ready,
        Disposing
    }
}
