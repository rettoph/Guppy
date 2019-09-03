using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public sealed class IsLoaderAttribute : GuppyAttribute
    {
        public IsLoaderAttribute(Int32 priority = 100) : base(priority)
        {
        }
    }
}
