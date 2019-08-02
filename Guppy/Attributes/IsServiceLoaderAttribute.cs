using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class IsServiceLoaderAttribute : GuppyAttribute
    {
        public IsServiceLoaderAttribute(ushort priority = 100) : base(priority)
        {
        }
    }
}
