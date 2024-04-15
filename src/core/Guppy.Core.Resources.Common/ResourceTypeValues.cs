using System.Text.Json;

namespace Guppy.Core.Resources.Common
{
    internal class ResourceTypeValues
    {
        public string Type { get; set; } = string.Empty;

        public Dictionary<string, JsonElement> Values { get; set; } = new Dictionary<string, JsonElement>();
    }
}
