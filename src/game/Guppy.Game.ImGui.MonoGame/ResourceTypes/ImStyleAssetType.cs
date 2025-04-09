using Guppy.Core.Assets.Common.AssetTypes;
using Guppy.Core.Serialization.Common.Services;
using Guppy.Game.ImGui.Common.Styling;

namespace Guppy.Game.ImGui.MonoGame.AssetTypes
{
    public class ImStyleAssetType(IJsonSerializationService json) : DefaultAssetType<ImStyle>(json)
    {
    }
}