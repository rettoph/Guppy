using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class IsLoaderAttribute : GuppyAttribute
    {
        public IsLoaderAttribute(int priority = 100) : base(priority)
        {
        }
    }
}
