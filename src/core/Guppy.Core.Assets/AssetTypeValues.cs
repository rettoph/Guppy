using System.Text.Json;

namespace Guppy.Core.Assets
{
    public class AssetTypeValues
    {
        public string Type { get; set; } = string.Empty;

        public Dictionary<string, JsonElement> Values { get; set; } = [];
    }
}