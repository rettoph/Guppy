﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Guppy.Network.Serialization.Json
{
    internal sealed class ByteNetIdJsonConverter : JsonConverter<NetId.Byte>
    {
        public override NetId.Byte Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            byte value = reader.ReadByte();

            return (NetId.Byte)NetId.Byte.Create(value);
        }

        public override void Write(Utf8JsonWriter writer, NetId.Byte value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Value);
        }
    }
}