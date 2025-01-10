using System.Diagnostics.CodeAnalysis;

namespace System.Text.Json
{
    public static class Utf8JsonReaderExtensions
    {
        private static bool FalseOrException(bool exception)
        {
            if (exception)
            {
                throw new JsonException();
            }

            return false;
        }

        public static bool CheckToken(ref this Utf8JsonReader reader, JsonTokenType tokenType, bool required)
        {
            if (reader.TokenType != tokenType)
            {
                return FalseOrException(required);
            }

            return true;
        }

        public static bool CheckPropertyName(ref this Utf8JsonReader reader, string value, bool required)
        {
            if (!reader.CheckToken(JsonTokenType.PropertyName, required))
            {
                return FalseOrException(required);
            }

            if (reader.GetString() != value)
            {
                return FalseOrException(required);
            }

            return true;
        }

        public static bool CheckProperty<T>(ref this Utf8JsonReader reader, T property, bool required)
            where T : Enum
        {
            return reader.CheckPropertyName(property.ToString(), required);
        }

        public static bool ReadPropertyName(ref this Utf8JsonReader reader, [MaybeNullWhen(false)] out string propertyName)
        {
            if (reader.CheckToken(JsonTokenType.StartObject, false))
            {
                reader.Read();
            }

            if (reader.CheckToken(JsonTokenType.EndObject, false))
            {
                propertyName = default;
                return false;
            }

            if (!reader.CheckToken(JsonTokenType.PropertyName, false))
            {
                propertyName = default;
                return false;
            }

            propertyName = reader.GetString() ?? throw new JsonException();
            reader.Read();

            return true;
        }

        public static bool ReadProperty<T>(ref this Utf8JsonReader reader, [MaybeNullWhen(false)] out T property)
            where T : struct, Enum
        {
            if (reader.ReadPropertyName(out string? propertyName))
            {
                property = Enum.Parse<T>(propertyName);
                return true;
            }

            property = default;
            return false;
        }

        public static ushort ReadUInt16(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.Number, true);
            ushort value = reader.GetUInt16();
            reader.Read();

            return value;
        }

        public static short ReadInt16(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.Number, true);
            short value = reader.GetInt16();
            reader.Read();

            return value;
        }

        public static uint ReadUInt32(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.Number, true);
            uint value = reader.GetUInt32();
            reader.Read();

            return value;
        }

        public static int ReadInt32(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.Number, true);
            int value = reader.GetInt32();
            reader.Read();

            return value;
        }

        public static long ReadInt64(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.Number, true);
            long value = reader.GetInt64();
            reader.Read();

            return value;
        }

        public static byte ReadByte(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.Number, true);
            byte value = reader.GetByte();
            reader.Read();

            return value;
        }

        public static float ReadSingle(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.Number, true);
            float value = reader.GetSingle();
            reader.Read();

            return value;
        }

        public static string ReadString(ref this Utf8JsonReader reader)
        {
            reader.CheckToken(JsonTokenType.String, true);
            string value = reader.GetString() ?? throw new JsonException();
            reader.Read();

            return value;
        }
    }
}