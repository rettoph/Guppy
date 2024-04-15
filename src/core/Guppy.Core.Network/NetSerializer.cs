using Guppy.Core.Network.Services;
using LiteNetLib.Utils;

namespace Guppy.Core.Network
{
    public abstract class NetSerializer<T> : INetSerializer<T>, INetSerializer
        where T : notnull
    {
        private INetId _id = default!;

        INetId INetSerializer.Id
        {
            get => _id;
            set => _id = value;
        }

        public INetId Id => _id;

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
