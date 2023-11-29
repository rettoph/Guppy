using Guppy.Attributes;
using Guppy.Common;
using System;
using System.Collections.Generic;
using System.CommandLine.Parsing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands.TokenPropertySetters
{
    [AutoLoad]
    internal class NullableEnumPropertySetter : ITokenPropertySetter
    {
        public bool AppliesTo(Type type)
        {
            return type.ImplementsGenericTypeDefinition(typeof(Nullable<>)) && Nullable.GetUnderlyingType(type)!.IsEnum;
        }

        public bool SetValue(PropertyInfo property, object instance, Token token)
        {
            if(Enum.TryParse(Nullable.GetUnderlyingType(property.PropertyType)!, token.Value, true, out var result))
            {
                property.SetValue(instance, result);
                return true;
            }

            return false;
        }
    }
}
