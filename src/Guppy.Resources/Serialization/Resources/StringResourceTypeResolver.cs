using Guppy.Attributes;
using Guppy.Resources;
using Guppy.Resources.Serialization.Resources;

namespace Guppy.MonoGame.Serialization.ResourceTypes
{
    [AutoLoad]
    internal class StringResourceTypeResolver : ResourceTypeResolver<string>
    {
        protected override bool TryResolve(Resource<string> resource, string input, out string value)
        {
            value = input;
            return true;
        }
    }
}
