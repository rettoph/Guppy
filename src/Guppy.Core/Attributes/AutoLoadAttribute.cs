using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class AutoLoadAttribute : Attribute
    {
        public readonly Int32 Order;

        #region Constructors
        public AutoLoadAttribute(Int32 order = 0)
        {
            this.Order = order;
        }
        #endregion
    }
}
