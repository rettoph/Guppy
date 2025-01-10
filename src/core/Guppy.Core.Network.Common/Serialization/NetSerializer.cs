using Guppy.Core.Network.Common.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network.Common.Serialization
{
    public abstract class NetSerializer<T> : INetSerializer<T>, INetSerializer
        where T : notnull
    {
        INetId INetSerializer.Id
        {
            get => this.Id;
            set => this.Id = value;
        }

        public INetId Id { get; private set; } = default!;

        public Type Type { get; } = typeof(T);


        public virtual void Initialize(INetSerializerService serializers)
        {
        }

        public abstract void Serialize(NetDataWriter writer, in T instance);

        public abstract T Deserialize(NetDataReader reader);

        void INetSerializer.Serialize(NetDataWriter writer, in object instance)
        {
            if (instance is T casted)
            {
                this.Serialize(writer, in casted);
                return;
            }

            throw new ArgumentException($"Unable to cast {instance.GetType().Name} to {typeof(T).Name}");
        }

        object INetSerializer.Deserialize(NetDataReader reader)
        {
            return this.Deserialize(reader);
        }
    }
}