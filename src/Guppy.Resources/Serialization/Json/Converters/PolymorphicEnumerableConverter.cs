using Guppy.Common.Collections;
using Guppy.Common.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json.Converters
{
    public class PolymorphicEnumerableConverter<T> : JsonConverter<IEnumerable<T>>
    {
        public const string TypePropertyKey = "Type";
        public const string ValuePropertyKey = "Value";

        private Map<string, Type> _types;

        public PolymorphicEnumerableConverter(IAssemblyProvider assembly, IEnumerable<PolymorphicJsonType> types)
        {
            var typeTuples = types.Where(x => x.Type.IsAssignableTo(typeof(T))).Select(x => (x.Key, x.Type));
            _types = new Map<string, Type>(typeTuples);
        }

        public override IEnumerable<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<T> items = new List<T>();
            
            while(reader.ReadPropertyName(out string? key))
            {
                if(!_types.TryGet(key, out Type? type))
                {
                    break;
                }

                var group = (IEnumerable<T>)JsonSerializer.Deserialize(ref reader, type.MakeArrayType(), options)!;
                items.AddRange(group);
                reader.Read();
            }

            return items;
        }

        public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if(value is null)
            {
                writer.WriteEndObject();
                return;
            }

            var groups = value.GroupBy(x => x.GetType()).ToDictionary(x => x.Key, x => x.ToArray());

            foreach((Type type, T[] instances) in groups)
            {
                if(type == typeof(T))
                { // Recursion detected
                    continue;
                }

                if(!_types.TryGet(type, out var key))
                {
                    continue;
                }

                writer.WritePropertyName(key);
                var array = Array.CreateInstance(type, instances.Length);
                Array.Copy(instances, array, instances.Length);
                JsonSerializer.Serialize(writer, array, array.GetType(), options);
            }

            writer.WriteEndObject();
        }
    }
}
