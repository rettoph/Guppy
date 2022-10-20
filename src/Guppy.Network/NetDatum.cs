using Guppy.Network.Providers;
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

        public abstract object? GetValue();

        internal abstract void Deserialize(NetDataReader reader, INetDatumProvider datum);
        internal abstract void Serialize(NetDataWriter writer, INetDatumProvider datum, bool sign, in object value);

        public abstract void Recycle();

        public void Dispose()
        {
            this.Recycle();
        }
    }

    public sealed class NetDatum<T> : NetDatum
    {
        public new readonly NetSerializer<T> Serializer;
        public new readonly NetDatumType<T> Type;
        public T Value;

        internal NetDatum(NetDatumType<T> type, NetSerializer<T> serializer) : base(type, serializer)
        {
            this.Type = type;
            this.Serializer = serializer;

            this.Value = default!; 
        }

        internal override void Serialize(NetDataWriter writer, INetDatumProvider datum, bool sign, in object value)
        {
            if(value is T casted)
            {
                this.Serialize(writer, datum, sign, in casted);
            }
        }

        internal void Serialize(NetDataWriter writer, INetDatumProvider datum, bool sign, in T value)
        {
            this.Value = value;

            if (sign)
            {
                this.Serializer.Id.Write(writer);
            }

            this.Serializer.Serialize(writer, datum, in this.Value);
        }

        internal override void Deserialize(NetDataReader reader, INetDatumProvider datum)
        {
            this.Serializer.Deserialize(reader, datum, out this.Value);
        }

        public override void Recycle()
        {
            this.Type.Recycle(this);
        }

        public override object? GetValue()
        {
            return this.Value;
        }
    }
}
