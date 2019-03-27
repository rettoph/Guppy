using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Attributes
{
    public class StylePropertyTypeAttribute : Attribute
    {
        private Type _type;

        public StylePropertyTypeAttribute(Type type)
        {
            _type = type;
        }

        public void Assert(Object value)
        {
            if (!_type.IsAssignableFrom(value.GetType()))
                throw new Exception($"Invalid StyleProperty Type given. Recieved {value.GetType().Name} but expected {_type.Name} => '{value.ToString()}'");
        }
    }
}
