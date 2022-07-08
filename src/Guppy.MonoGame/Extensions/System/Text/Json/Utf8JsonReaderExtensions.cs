﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace System.Text.Json
{
    public static class Utf8JsonReaderExtensions
    {
        public static bool CheckPropertyName(ref this Utf8JsonReader reader, string value, bool required)
        {
            if (reader.TokenType != JsonTokenType.PropertyName || reader.GetString() != value)
            {
                if (required)
                    throw new JsonException();

                return false;
            }

            return true;
        }

        public static bool CheckProperty<T>(ref this Utf8JsonReader reader, T property, bool required)
            where T : Enum
        {
            return reader.CheckPropertyName(property.ToString(), required);
        }

        public static bool CheckToken(ref this Utf8JsonReader reader, JsonTokenType tokenType, bool required)
        {
            if (reader.TokenType != tokenType)
            {
                if (required)
                    throw new JsonException();

                return false;
            }

            return true;
        }

        public static bool ReadPropertyName(ref this Utf8JsonReader reader, [MaybeNullWhen(false)] out string propertyName)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                propertyName = default;
                return false;
            }

            propertyName = reader.GetString() ?? throw new JsonException();
            reader.Read();

            return true;
        }

        public static bool ReadProperty<T>(ref this Utf8JsonReader reader, [MaybeNullWhen(false)] out T property)
            where T : Enum
        {
            if (reader.ReadPropertyName(out string? propertyName))
            {
                property = (T)Enum.Parse(typeof(T), propertyName);
                return true;
            }

            property = default;
            return false;
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
