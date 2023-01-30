using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Guppy.Resources.Serialization.Json;
using STJ = System.Text.Json;

namespace Guppy.Resources.Serialization.Json.Implementations
{
    internal sealed class JsonSerializer : IJsonSerializer
    {
        private STJ.JsonSerializerOptions _options;

        public JsonSerializer(IEnumerable<JsonConverter> converters)
        {
            _options = new STJ.JsonSerializerOptions()
            {
                WriteIndented = true
            };

            foreach (JsonConverter converter in converters)
            {
                _options.Converters.Add(converter);
            }
        }

        public T? Deserialize<T>(string json)
        {
            return STJ.JsonSerializer.Deserialize<T>(json, _options);
        }

        public T? Deserialize<T>(Stream utf8Json)
        {
            return STJ.JsonSerializer.Deserialize<T>(utf8Json, _options);
        }

        public string Serialize<T>(T obj)
        {
            return STJ.JsonSerializer.Serialize(obj, _options);
        }
        public void Serialize<T>(Stream utf8Json, T obj)
        {
            STJ.JsonSerializer.Serialize(utf8Json, obj, _options);
        }
    }
}
