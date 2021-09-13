using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Attributes
{
    public sealed class ManualInitializationAttribute : Attribute
    {
        public readonly Boolean Value;

        public ManualInitializationAttribute(Boolean value)
        {
            this.Value = value;
        }
        public ManualInitializationAttribute() : this(true)
        {

        }
    }
}
