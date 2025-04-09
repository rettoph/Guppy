using Guppy.Core.Files.Common;
using Guppy.Core.Assets.Common;
using Guppy.Core.Assets.Common.AssetTypes;

namespace Guppy.Core.Assets.AssetTypes
{
    internal class StringAssetType : SimpleAssetType<string>
    {
        protected override bool TryResolve(AssetKey<string> resource, DirectoryPath root, string input, out string value)
        {
            value = input;
            return true;
        }
    }
}