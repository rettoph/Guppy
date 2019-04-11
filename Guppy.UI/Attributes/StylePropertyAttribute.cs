using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Attributes
{
    public class StylePropertyAttribute : Attribute
    {
        public readonly Type Type;
        public readonly Boolean Inherit;

        public StylePropertyAttribute(Type type, Boolean inherit)
        {
            this.Type = type;
            this.Inherit = inherit;
        }

        public void Assert(Type type)
        {
            if (!this.Type.IsAssignableFrom(type))
                throw new Exception($"Unable to update style property! {this.Type.Name} expected, but {type.Name} recieved!");
        }
    }
}
