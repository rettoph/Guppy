using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using DrawingColor = System.Drawing.Color;

namespace Guppy.Game.Serialization.Json.Converters
{
    internal sealed class ColorConverter : JsonConverter<Color>
    {
        private enum Properties
        {
            R,
            G,
            B,
            A
        }

        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(Color);

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber(Properties.R, value.R);
            writer.WriteNumber(Properties.G, value.G);
            writer.WriteNumber(Properties.B, value.B);
            writer.WriteNumber(Properties.A, value.A);

            writer.WriteEndObject();
        }

        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.TokenType switch
        {
            JsonTokenType.StartObject => ReadColorObject(ref reader, typeToConvert, options),
            JsonTokenType.StartArray => ReadColorArray(ref reader, typeToConvert, options),
            JsonTokenType.String => ReadColorString(ref reader, typeToConvert, options),
            JsonTokenType.Number => ReadColorNumber(ref reader, typeToConvert, options),
            _ => throw new JsonException(),
        };

#pragma warning disable IDE0060 // Remove unused parameter
        private static Color ReadColorNumber(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            reader.CheckToken(JsonTokenType.Number, true);

            return new Color(reader.GetUInt32());
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private static Color ReadColorString(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            reader.CheckToken(JsonTokenType.String, true);

            string value = reader.GetString() ?? throw new Exception();

            DrawingColor drawingColor = System.Drawing.ColorTranslator.FromHtml(value);
            return System.Drawing.ColorExtensions.ToXnaColor(drawingColor);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private static Color ReadColorArray(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            byte a = 255;

            reader.CheckToken(JsonTokenType.StartArray, true);
            reader.Read();

            byte r = reader.ReadByte();
            byte g = reader.ReadByte();
            byte b = reader.ReadByte();

            if (reader.CheckToken(JsonTokenType.Number, false))
            {
                a = reader.ReadByte();
            }

            reader.CheckToken(JsonTokenType.EndArray, true);

            return new Color(r, g, b, a);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private static Color ReadColorObject(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
#pragma warning restore IDE0060 // Remove unused parameter
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