using Guppy.Attributes;
using Guppy.Resources;

namespace Guppy.Resources.ResourceTypes
{
    [AutoLoad]
    internal class StringResourceType : ResourceType<string>
    {
        protected override bool TryResolve(Resource<string> resource, string input, string root, out string value)
        {
            value = input;
            return true;
        }
    }
}
