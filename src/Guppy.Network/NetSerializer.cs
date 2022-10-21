using Guppy.Common.Collections;
using Guppy.Network.Delegates;
using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
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


        public virtual void Initialize(INetSerializerProvider provider)
        {
        }

        public abstract void Serialize(NetDataWriter writer, INetSerializerProvider serializers, in T instance);

        public abstract T Deserialize(NetDataReader reader, INetSerializerProvider serializers);

        void INetSerializer.Serialize(NetDataWriter writer, INetSerializerProvider serializers, in object instance)
        {
            if(instance is T casted)
            {
                this.Serialize(writer, serializers, in casted);
                return;
            }

            throw new ArgumentException(nameof(instance));
        }

        object INetSerializer.Deserialize(NetDataReader reader, INetSerializerProvider serializers)
        {
            return this.Deserialize(reader, serializers);
        }
    }
}
