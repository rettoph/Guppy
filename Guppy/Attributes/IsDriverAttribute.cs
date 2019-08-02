using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    /// <summary>
    /// Used to mark a specific type as a driver.
    /// Any driver with this attribute will automatically
    /// be registered via the DriverServiceLoader
    /// </summary>
    public class IsDriverAttribute : GuppyAttribute
    {
        public readonly Type DrivenType;

        public IsDriverAttribute(Type targetType, UInt16 priority = 100) : base(priority)
        {
            this.DrivenType = targetType;
        }
    }
}
