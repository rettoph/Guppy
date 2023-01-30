using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Resources.Serialization.Json.Converters
{
    internal sealed class PackConverter : JsonConverter<Pack>
    {
        private const string IdKey = "Id";
        private const string NameKey = "Name";
        private const string ResourcesKey = "Resources";

        public override Pack? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Guid id = default!;
            string name = default!;
            Dictionary<string, IResourceCollection> resources = default!;

            while(reader.ReadPropertyName(out string? property))
            {
                switch(property)
                {
                    case IdKey:
                        id = JsonSerializer.Deserialize<Guid>(ref reader, options);
                        break;
                    case NameKey:
                        name = reader.GetString()!;
                        break;
                    case ResourcesKey:
                        resources = JsonSerializer.Deserialize<Dictionary<string, IResourceCollection>>(ref reader, options)!;
                        break;
                }

                reader.Read();
            }

            var pack = new Pack(id, name, resources);
            return pack;
        }

        public override void Write(Utf8JsonWriter writer, Pack value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(IdKey, value.Id);
            writer.WriteString(NameKey, value.Name);

            writer.WritePropertyName(ResourcesKey);
            JsonSerializer.Serialize(writer, value.localized, options);

            writer.WriteEndObject();
        }
    }
}
