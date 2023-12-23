using Guppy.Attributes;
using Guppy.Enums;
using System.CommandLine.Parsing;
using System.Reflection;

namespace Guppy.Commands.TokenPropertySetters
{
    [Service<ITokenPropertySetter>(ServiceLifetime.Scoped, true)]
    public interface ITokenPropertySetter
    {
        bool AppliesTo(Type type);

        bool SetValue(PropertyInfo property, object instance, Token token);
    }
}
