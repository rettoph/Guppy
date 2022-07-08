using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DrawingColor = System.Drawing.Color;

namespace Guppy.MonoGame.Json.JsonConverters
{
    public sealed class ColorJsonConverter : JsonConverter<Color>
    {
        private enum Properties
        {
            R,
            G,
            B,
            A
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(Color);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber(Properties.R, value.R);
            writer.WriteNumber(Properties.G, value.G);
            writer.WriteNumber(Properties.B, value.B);
            writer.WriteNumber(Properties.A, value.A);

            writer.WriteEndObject();
        }

        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch(reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    return this.ReadColorObject(ref reader, typeToConvert, options);
                case JsonTokenType.StartArray:
                    return this.ReadColorArray(ref reader, typeToConvert, options);
                case JsonTokenType.String:
                    return this.ReadColorString(ref reader, typeToConvert, options);
                case JsonTokenType.Number:
                    return this.ReadColorNumber(ref reader, typeToConvert, options);

            }

            throw new JsonException();
        }

        private Color ReadColorNumber(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.CheckToken(JsonTokenType.Number, true);

            return new Color(reader.GetUInt32());
        }

        private Color ReadColorString(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            reader.CheckToken(JsonTokenType.String, true);

            var htmlValue = reader.GetString();
            var drawingColor = System.Drawing.ColorTranslator.FromHtml(htmlValue);
            return System.Drawing.ColorExtensions.ToXnaColor(drawingColor);
        }

        private Color ReadColorArray(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            byte r = default, g = default, b = default, a = 255;

            reader.CheckToken(JsonTokenType.StartArray, true);
            reader.Read();

            r = reader.ReadByte();
            g = reader.ReadByte();
            b = reader.ReadByte();

            if(reader.CheckToken(JsonTokenType.Number, false))
            {
                a = reader.ReadByte();
            }

            reader.CheckToken(JsonTokenType.EndArray, true);

            return new Color(r, g, b, a);
        }

        private Color ReadColorObject(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            byte r = default, g = default, b = default, a = default;

            reader.CheckToken(JsonTokenType.StartObject, true);
            reader.Read();

            while (reader.ReadProperty(out Properties property))
            {
                switch (property)
                {
                    case Properties.R:
                        r = reader.ReadByte();
                        break;
                    case Properties.G:
                        g = reader.ReadByte();
                        break;
                    case Properties.B:
                        b = reader.ReadByte();
                        break;
                    case Properties.A:
                        a = reader.ReadByte();
                        break;
                }

            }

            reader.CheckToken(JsonTokenType.EndObject, true);

            return new Color(r, g, b, a);
        }
    }
}
