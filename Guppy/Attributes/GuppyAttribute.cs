using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public abstract class GuppyAttribute : Attribute
    {
        public Int32 Priority { get; private set; }

        public GuppyAttribute(Int32 priority = 100)
        {
            this.Priority = priority;
        }
    }
}
