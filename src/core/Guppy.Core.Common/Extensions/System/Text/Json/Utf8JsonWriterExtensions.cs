namespace System.Text.Json
{
    public static class Utf8JsonWriterExtensions
    {
        public static void WritePropertyName(this Utf8JsonWriter writer, Enum property)
        {
            writer.WritePropertyName(property.ToString());
        }

        public static void WriteString(this Utf8JsonWriter writer, Enum property, string value)
        {
            writer.WriteString(property.ToString(), value);
        }

        public static void WriteNumber(this Utf8JsonWriter writer, Enum property, byte value)
        {
            writer.WriteNumber(property.ToString(), value);
        }

        public static void WriteNumber(this Utf8JsonWriter writer, Enum property, int value)
        {
            writer.WriteNumber(property.ToString(), value);
        }

        public static void WriteNumber(this Utf8JsonWriter writer, Enum property, float value)
        {
            writer.WriteNumber(property.ToString(), value);
        }
    }
}