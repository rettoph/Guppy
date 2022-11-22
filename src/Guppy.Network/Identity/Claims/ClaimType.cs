using Guppy.Common.Collections;
using Guppy.Network.Identity.Enums;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Guppy.Network.Identity.Claims
{
    public abstract class ClaimType
    {
        private static DoubleDictionary<string, Type, ClaimType> _types;

        public static IEnumerable<ClaimType> Items => _types.Values;

        static ClaimType()
        {
            _types = new DoubleDictionary<string, Type, ClaimType>();

            ClaimType.Register<string>((w, v) => w.Put(v), r => r.GetString());
            ClaimType.Register<byte>((w, v) => w.Put(v), r => r.GetByte());
            ClaimType.Register<float>((w, v) => w.Put(v), r => r.GetFloat());
            ClaimType.Register<double>((w, v) => w.Put(v), r => r.GetDouble());
            ClaimType.Register<short>((w, v) => w.Put(v), r => r.GetShort());
            ClaimType.Register<ushort>((w, v) => w.Put(v), r => r.GetUShort());
            ClaimType.Register<int>((w, v) => w.Put(v), r => r.GetInt());
            ClaimType.Register<uint>((w, v) => w.Put(v), r => r.GetUInt());
            ClaimType.Register<long>((w, v) => w.Put(v), r => r.GetLong());
            ClaimType.Register<ulong>((w, v) => w.Put(v), r => r.GetULong());
        }

        public static ClaimType<T> Register<T>(Action<NetDataWriter, T> netSerializer, Func<NetDataReader, T> netDeserializer)
        {
            var type = new ClaimType<T>(typeof(T).Name!, netSerializer, netDeserializer);
            _types.TryAdd(type.Name, type.Type, type);

            return type;
        }

        public static ClaimType<T> Get<T>()
        {
            return (ClaimType<T>)_types[typeof(T)];
        }

        public static ClaimType Get(string name)
        {
            return _types[name];
        }

        public readonly string Name;
        public readonly Type Type;

        protected internal ClaimType(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        public abstract void SerializeValue(NetDataWriter writer, object? value);

        public abstract object DeserializeValue(NetDataReader reader);

        public abstract Claim Create(string key, object value, ClaimAccessibility accessibility);
    }

    public sealed class ClaimType<T> : ClaimType
    {
        private readonly Action<NetDataWriter, T> _netSerializer;
        private readonly Func<NetDataReader, T> _netDeserializer;

        public ClaimType(string name, Action<NetDataWriter, T> serializer, Func<NetDataReader, T> deserializer) : base(name, typeof(T))
        {
            _netSerializer = serializer;
            _netDeserializer = deserializer;
        }

        public override void SerializeValue(NetDataWriter writer, object? value)
        {
            if (value is T casted)
            {
                _netSerializer(writer, casted);
                return;
            }

            throw new ArgumentException(nameof(value));
        }

        public override object DeserializeValue(NetDataReader reader)
        {
            return _netDeserializer(reader) ?? throw new NullReferenceException();
        }

        public override Claim Create(string key, object value, ClaimAccessibility accessibility)
        {
            if (value is T casted)
            {
                return new Claim<T>(key, casted, accessibility);
            }

            throw new ArgumentException(nameof(value));
        }
    }
}
