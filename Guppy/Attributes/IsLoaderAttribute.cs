using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    /// <summary>
    /// Used to identify a class that is a loader
    /// </summary>
    public class IsLoaderAttribute : GuppyAttribute
    {
        public IsLoaderAttribute(UInt16 priority = 100) : base(priority)
        {
        }
    }
}
