using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Enums
{
    public enum InitializationStatus : Byte
    {
        NotReady,
        Booting,
        PreInitializing,
        Initializing,
        PostInitializing,
        Ready
    }
}
