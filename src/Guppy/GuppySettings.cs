using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    /// <summary>
    /// Simple static class that contains global setting values
    /// </summary>
    public static class GuppySettings
    {
        /// <summary>
        /// The maximum pool size for a service factory pool.
        /// </summary>
        public static Int32 MaxServicePoolSize { get; set; } = 256;
    }
}
