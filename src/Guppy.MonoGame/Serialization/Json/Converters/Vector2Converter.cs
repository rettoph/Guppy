using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Serialization.Json.Converters
{
    internal sealed class Vector2Converter : JsonConverter<Vector2>
    {
        public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Vector2 output = Vector2.Zero;

            while(reader.ReadPropertyName(out string? property))
            {
                switch(property)
                {
                    case nameof(Vector2.X):
                        output.X = reader.ReadSingle();
                        break;
                    case nameof(Vector2.Y):
                        output.Y = reader.ReadSingle();
                        break;
                }
            }

            return output;
        }

        public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Vector2.X), value.X);
            writer.WriteNumber(nameof(Vector2.Y), value.Y);

            writer.WriteEndObject();
        }
    }
}
