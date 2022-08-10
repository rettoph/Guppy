using Guppy.Common.Collections;
using Guppy.Network.Providers;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public abstract class NetDatumType
    {
        public readonly NetSerializer Serializer;

        internal NetDatumType(NetSerializer serializer)
        {
            this.Serializer = serializer;
        }

        public abstract NetDatum Create();
    }

    public sealed class NetDatumType<TValue> : NetDatumType
    {
        private Factory<NetDatum<TValue>> _factory;

        public new NetSerializer<TValue> Serializer;

        public NetDatumType(NetSerializer<TValue> serializer) : base(serializer)
        {
            _factory = new Factory<NetDatum<TValue>>(this.FactoryMethod);

            this.Serializer = serializer;
        }

        private NetDatum<TValue> FactoryMethod()
        {
            return new NetDatum<TValue>(this, this.Serializer);
        }

        public override NetDatum<TValue> Create()
        {
            return _factory.GetInstance();
        }

        internal void Recycle(NetDatum<TValue> data)
        {
            _factory.TryReturnToPool(data);
        }
    }
}
