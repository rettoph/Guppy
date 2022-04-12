using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetDeserialized
    {
        public readonly Type Type;

        protected NetDeserialized(Type type)
        {
            this.Type = type;
        }

        public abstract void Recycle();
    }

    public class NetDeserialized<T> : NetDeserialized
    {
        public T? Instance = default;

        public readonly NetSerializer<T> Serializer;

        internal NetDeserialized(
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
