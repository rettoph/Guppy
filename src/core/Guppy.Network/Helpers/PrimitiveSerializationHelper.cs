using LiteNetLib.Utils;

namespace Guppy.Network.Helpers
{
    public static class PrimitiveSerializationHelper
    {
        public static void SerializeInt32(NetDataWriter writer, in int value)
        {
            writer.Put(value);
        }

        public static void SerializeUInt32(NetDataWriter writer, in uint value)
        {
            writer.Put(value);
        }

        public static void SerializeSingle(NetDataWriter writer, in float value)
        {
            writer.Put(value);
        }

        public static void SerializeInt16(NetDataWriter writer, in short value)
        {
            writer.Put(value);
        }

        public static void SerializeUInt16(NetDataWriter writer, in ushort value)
        {
            writer.Put(value);
        }

        public static void SerializeByte(NetDataWriter writer, in byte value)
        {
            writer.Put(value);
        }

        public static void DeserializeInt32(NetDataReader reader, out int value)
        {
            value = reader.GetInt();
        }

        public static void DeserializeUInt32(NetDataReader reader, out uint value)
        {
            value = reader.GetUInt();
        }

        public static void DeserializeSingle(NetDataReader reader, out float value)
        {
            value = reader.GetFloat();
        }

        public static void DeserializeInt16(NetDataReader reader, out short value)
        {
            value = reader.GetShort();
        }

        public static void DeserializeUInt16(NetDataReader reader, out ushort value)
        {
            value = reader.GetUShort();
        }

        public static void DeserializeByte(NetDataReader reader, out byte value)
        {
            value = reader.GetByte();
        }
    }
}
