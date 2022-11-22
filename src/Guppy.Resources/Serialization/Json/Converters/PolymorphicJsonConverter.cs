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
    public sealed class PolymorphicJsonConverter<T> : JsonConverter<T>
        where T : notnull
    {
        public const string TypePropertyKey = "Type";
        public const string ValuePropertyKey = "Value";

        private Map<string, Type> _types;

        public PolymorphicJsonConverter(IAssemblyProvider assembly, IEnumerable<PolymorphicJsonType> types)
        {
            var typeTuples = assembly.GetTypes<T>(x => !x.IsInterface && !x.IsAbstract).Select(x => (types.FirstOrDefault(t => t.Type == x)?.Key ?? x.Name!, x));
            _types = new Map<string, Type>(typeTuples);
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            Type implementationType = value.GetType();

            if(implementationType == typeof(T))
            { // Recursion detected
                return;
            }

            writer.WriteStartObject();

            writer.WriteString(TypePropertyKey, _types[implementationType]);

            writer.WritePropertyName(ValuePropertyKey);
            JsonSerializer.Serialize(writer, value, implementationType, options);

            writer.WriteEndObject();
        }
    }
}
