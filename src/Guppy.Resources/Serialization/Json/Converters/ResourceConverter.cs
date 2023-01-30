using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json.Converters
{
    public abstract class ResourceConverter<TValue, TJson> : JsonConverter<IResource<TValue, TJson>>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            var result = typeof(IResource<TValue, TJson>).IsAssignableFrom(typeToConvert);
            return result;
        }

        public override IResource<TValue, TJson>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string name = default!;
            TJson json= default(TJson)!;

            while(reader.ReadPropertyName(out string? property))
            {
                switch(property)
                {
                    case nameof(IResource.Name):
                        name = reader.GetString()!;
                        break;

                    case nameof(IResource<object, TJson>.Value):
                        json = JsonSerializer.Deserialize<TJson>(ref reader, options)!;
                        break;
                }

                reader.Read();
            }

            return this.Factory(name, json);
        }

        public override void Write(Utf8JsonWriter writer, IResource<TValue, TJson> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(value.Name), value.Name);

            var json = value.GetJson();
            writer.WritePropertyName(nameof(value.Value));
            JsonSerializer.Serialize(writer, json, options);

            writer.WriteEndObject();

        }

        public abstract IResource<TValue, TJson> Factory(string name, TJson json);
    }
}
