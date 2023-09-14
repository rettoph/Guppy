using Guppy.Common.Collections;
using Guppy.Common.Providers;
using Guppy.Resources.Serialization.Json.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Guppy.Resources.Serialization.Json.Converters
{
    public sealed class DictionaryPolymorphicConverter<T> : JsonConverter<Dictionary<string, T>>
        where T : notnull
    {
        private Map<string, Type> _types;

        public DictionaryPolymorphicConverter(IAssemblyProvider assembly, IEnumerable<PolymorphicJsonType> types)
        {
            var typeTuples = types.Where(x => x.Type.IsAssignableTo(typeof(T))).Select(x => (x.Key, x.Type));
            _types = new Map<string, Type>(typeTuples);
        }

        public override Dictionary<string, T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Dictionary<string, T> result = new Dictionary<string, T>();

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadPropertyName(out string? propertyName))
            {
                Type type = _types[propertyName];
                T instance = (T)JsonSerializer.Deserialize(ref reader, type, options)!;
                reader.Read();

                result.Add(propertyName, instance);
            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return result;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, T> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
