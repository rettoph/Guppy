using Guppy.Engine.Attributes;
using Guppy.Core.Files;

namespace Guppy.Core.Resources.ResourceTypes
{
    [AutoLoad]
    internal class StringResourceType : SimpleResourceType<string>
    {
        protected override bool TryResolve(Resource<string> resource, DirectoryLocation root, string input, out string value)
        {
            value = input;
            return true;
        }
    }
}
