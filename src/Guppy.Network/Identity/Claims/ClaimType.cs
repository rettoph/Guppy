using Guppy.Common.Collections;
using Guppy.Network.Identity.Enums;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            ClaimType.Register<string>("String", (w, v) => w.Put(v), r => r.GetString());
            ClaimType.Register<byte>("Byte", (w, v) => w.Put(v), r => r.GetByte());
            ClaimType.Register<float>("Single", (w, v) => w.Put(v), r => r.GetFloat());
            ClaimType.Register<double>("Double", (w, v) => w.Put(v), r => r.GetDouble());
            ClaimType.Register<short>("Int16", (w, v) => w.Put(v), r => r.GetShort());
            ClaimType.Register<ushort>("UInt16", (w, v) => w.Put(v), r => r.GetUShort());
            ClaimType.Register<int>("Int32", (w, v) => w.Put(v), r => r.GetInt());
            ClaimType.Register<uint>("UInt32", (w, v) => w.Put(v), r => r.GetUInt());
            ClaimType.Register<long>("Int64", (w, v) => w.Put(v), r => r.GetLong());
            ClaimType.Register<ulong>("UInt64", (w, v) => w.Put(v), r => r.GetULong());
        }

        public static ClaimType<T> Register<T>(string name, Action<NetDataWriter, T> serializer, Func<NetDataReader, T> deserializer)
        {
            var type = new ClaimType<T>(name, serializer, deserializer);
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

        public abstract void SerializeValue(NetDataWriter writer, object value);

        public abstract object DeserializeValue(NetDataReader reader);

        public abstract Claim Create(string key, object value, ClaimAccessibility accessibility);
    }

    public sealed class ClaimType<T> : ClaimType
    {
        private readonly Action<NetDataWriter, T> _serializer;
        private readonly Func<NetDataReader, T> _deserializer;

        public ClaimType(string name, Action<NetDataWriter, T> serializer, Func<NetDataReader, T> deserializer) : base(name, typeof(T))
        {
            _serializer = serializer;
            _deserializer = deserializer;
        }

        public override void SerializeValue(NetDataWriter writer, object value)
        {
            if (value is T casted)
            {
                _serializer(writer, casted);
                return;
            }

            throw new ArgumentException(nameof(value));
        }

        public override object DeserializeValue(NetDataReader reader)
        {
            return _deserializer(reader) ?? throw new NullReferenceException();
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
