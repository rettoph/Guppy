using Guppy.Core.Common.Attributes;
using Guppy.Core.Files.Common;

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
