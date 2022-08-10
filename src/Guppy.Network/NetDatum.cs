using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetDatum : IRecyclable
    {
        public readonly NetSerializer Serializer;
        public readonly NetDatumType Type;

        internal NetDatum(NetDatumType type, NetSerializer serializer)
        {
            this.Type = type;
            this.Serializer = serializer;
        }

        internal abstract void Serialize(NetDataWriter writer);

        internal abstract void Deserialize(NetDataReader reader);

        public abstract void Recycle();

        public void Dispose()
        {
            this.Recycle();
        }
    }

    public sealed class NetDatum<TValue> : NetDatum
    {
        public new readonly NetSerializer<TValue> Serializer;
        public new readonly NetDatumType<TValue> Type;
        public TValue Value;

        internal NetDatum(NetDatumType<TValue> type, NetSerializer<TValue> serializer) : base(type, serializer)
        {
            this.Type = type;
            this.Serializer = serializer;

            this.Value = default!; 
        }

        internal override void Serialize(NetDataWriter writer)
        {
            this.Serializer.Serialize(writer, in this.Value);
        }

        internal override void Deserialize(NetDataReader reader)
        {
            this.Serializer.Deserialize(reader, out this.Value);
        }

        public override void Recycle()
        {
            this.Type.Recycle(this);
        }
    }
}
