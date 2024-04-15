using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
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
