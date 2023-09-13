using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Serialization
{
    internal class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options;

        public JsonSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }
    }
}
