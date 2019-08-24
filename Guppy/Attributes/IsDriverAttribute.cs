using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class IsDriverAttribute : GuppyAttribute
    {
        public Type Driven { get; private set; }

        public IsDriverAttribute(Type driven, Int32 priority = 100) : base(priority)
        {
            this.Driven = driven;
        }
    }
}
