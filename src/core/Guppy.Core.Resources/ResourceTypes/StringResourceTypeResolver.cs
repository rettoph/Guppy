using Guppy.Core.Files.Common;
using Guppy.Core.Resources.Common;
using Guppy.Core.Resources.Common.ResourceTypes;

namespace Guppy.Core.Resources.ResourceTypes
{
    internal class StringResourceType : SimpleResourceType<string>
    {
        protected override bool TryResolve(ResourceKey<string> resource, DirectoryLocation root, string input, out string value)
        {
            value = input;
            return true;
        }
    }
}