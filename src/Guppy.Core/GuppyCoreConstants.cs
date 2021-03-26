using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public static class GuppyCoreConstants
    {
        public static class Priorities
        {
            public const Int32 PreCreate = -10;
            public const Int32 Create = 10;
            public const Int32 PostCreate = 20;

            public const Int32 PreInitialize = -10;
            public const Int32 Initialize = 10;
            public const Int32 PostInitialize = 20;
        }
    }
}
