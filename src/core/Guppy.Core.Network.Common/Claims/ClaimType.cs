using Guppy.Core.Common.Collections;
using Guppy.Core.Network.Common.Identity.Enums;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Claims
{
    public abstract class ClaimType
    {
        private static DoubleDictionary<string, Type, ClaimType> _types;

        public static IEnumerable<ClaimType> Items => _types.Values;

        static ClaimType()
        {
            _types = new DoubleDictionary<string, Type, ClaimType>();

            Register((w, v) => w.Put(v), r => r.GetString());
            Register((w, v) => w.Put(v), r => r.GetByte());
            Register((w, v) => w.Put(v), r => r.GetFloat());
            Register((w, v) => w.Put(v), r => r.GetDouble());
            Register((w, v) => w.Put(v), r => r.GetShort());
            Register((w, v) => w.Put(v), r => r.GetUShort());
            Register((w, v) => w.Put(v), r => r.GetInt());
            Register((w, v) => w.Put(v), r => r.GetUInt());
            Register((w, v) => w.Put(v), r => r.GetLong());
            Register((w, v) => w.Put(v), r => r.GetULong());
            Register((w, v) => w.Put(v), r => r.GetEnum<UserType>());
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
            Name = name;
            Type = type;
        }

        public abstract void SerializeValue(NetDataWriter writer, object? value);

        public abstract object DeserializeValue(NetDataReader reader);

        public abstract Claim Create(string key, object value, ClaimAccessibility accessibility, DateTime? createdAt = null);
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

        public override Claim Create(string key, object value, ClaimAccessibility accessibility, DateTime? createdAt = null)
        {
            if (value is T casted)
            {
                return new Claim<T>(key, casted, accessibility, createdAt);
            }

            throw new ArgumentException(nameof(value));
        }
    }
}
