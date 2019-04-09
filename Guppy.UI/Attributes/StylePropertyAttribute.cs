using Guppy.UI.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StylePropertyAttribute : Attribute
    {
        public readonly Type Type;
        public readonly Boolean Inherit;

        public StylePropertyAttribute(Type type, Boolean inherit)
        {
            this.Type = type;
            this.Inherit = inherit;
        }

        internal void Assert(Object value)
        {
            if (!this.Type.IsAssignableFrom(value.GetType()))
                throw new Exception($"Invalid StyleProperty Type given. Recieved {value.GetType().Name} but expected {this.Type.Name} => '{value.ToString()}'");
        }
    }
}
