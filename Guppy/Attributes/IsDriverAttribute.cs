using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public class IsDriverAttribute : GuppyAttribute
    {
        public readonly Type Driven;

        public IsDriverAttribute(Type driven)
        {
            this.Driven = driven;
        }
    }
}
