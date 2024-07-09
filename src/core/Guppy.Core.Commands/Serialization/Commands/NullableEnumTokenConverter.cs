﻿using Guppy.Core.Commands.Common.Serialization.Commands;
using Guppy.Core.Common.Attributes;

namespace Guppy.Core.Commands.Serialization.Commands
{
    [AutoLoad]
    public class NullableEnumTokenConverter : ICommandTokenConverter
    {
        public bool AppliesTo(Type type)
        {
            return type.ImplementsGenericTypeDefinition(typeof(Nullable<>)) && Nullable.GetUnderlyingType(type)!.IsEnum;
        }

        public object? Deserialize(Type type, string token)
        {
            if (Enum.TryParse(Nullable.GetUnderlyingType(type)!, token, true, out object? result))
            {
                return result;
            }

            return null;
        }
    }
}
