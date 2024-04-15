using Guppy.Engine.Attributes;
using Guppy.Engine.Enums;
using System.CommandLine.Parsing;
using System.Reflection;

namespace Guppy.Game.Commands.TokenPropertySetters
{
    [Service<ITokenPropertySetter>(ServiceLifetime.Scoped, true)]
    public interface ITokenPropertySetter
    {
        bool AppliesTo(Type type);

        bool SetValue(PropertyInfo property, object instance, Token token);
    }
}
