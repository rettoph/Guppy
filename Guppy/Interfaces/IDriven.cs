using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// An object that has custom driver instances defined.
    /// 
    /// Drivers may be used to extend object functionality.
    /// </summary>
    public interface IDriven : IFrameable
    {
        /// <summary>
        /// List of all drivers within the current object
        /// </summary>
        IEnumerable<IDriver> Drivers { get; }

        /// <summary>
        /// Get a specific driver that extends the input generic type
        /// </summary>
        /// <typeparam name="TDriver"></typeparam>
        /// <returns></returns>
        TDriver GetDriver<TDriver>()
            where TDriver : IDriver;
    }
}
