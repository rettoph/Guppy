using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class GuppyAttribute : Attribute
    {
        public readonly UInt16 Priority;

        public GuppyAttribute(UInt16 priority = 100)
        {
            this.Priority = priority;
        }
    }
}
