using System.Text.Json;

namespace Guppy.Core.Resources
{
    public class ResourceTypeValues
    {
        public string Type { get; set; } = string.Empty;

        public Dictionary<string, JsonElement> Values { get; set; } = [];
    }
}
