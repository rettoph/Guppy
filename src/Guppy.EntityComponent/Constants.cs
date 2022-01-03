using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.EntityComponent
{
    public static class Constants
    {
        public static class Priorities
        {
            public const Int32 PreInitialize = -10;
            public const Int32 Initialize = 10;
            public const Int32 PostInitialize = 20;
        }

        public static class Orders
        {
            public const Int32 ComponentOrder = -10;
        }
    }
}
