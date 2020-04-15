using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class AutoLoadAttribute : Attribute
    {
        public readonly Int32 Priority;

        #region Constructors
        public AutoLoadAttribute(Int32 priority = 100)
        {
            this.Priority = priority;
        }
        #endregion
    }
}
