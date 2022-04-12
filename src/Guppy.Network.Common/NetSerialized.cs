using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetSerialized
    {
        public readonly Type Type;

        protected NetSerialized(Type type)
        {
            this.Type = type;
        }

        public abstract void Recycle();
    }

    public class NetSerialized<T> : NetSerialized
    {
        public T? Instance = default;

        public readonly NetSerializer<T> Serializer;

        public NetSerialized(
            NetSerializer<T> serializer) : base(typeof(T))
        {
            this.Serializer = serializer;
        }

        public override void Recycle()
        {
            this.Serializer.TryRecycle(this);
        }
    }
}
