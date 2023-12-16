using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    internal class ResourceTypeValues
    {
        public string Type { get; set; } = string.Empty;

        public Dictionary<string, JsonElement> Values { get; set; } = new Dictionary<string, JsonElement>(); 
    }
}
