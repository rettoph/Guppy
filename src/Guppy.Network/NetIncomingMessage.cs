using Guppy.Network.Providers;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class NetIncomingMessage<TBody> : NetMessage<TBody>, INetIncomingMessage
    {
        private readonly NetSerializer<TBody> _serializer;
        private readonly INetDatumProvider _dataProvider;
        private List<NetDatum> _data;

        public new readonly NetMessageType<TBody> Type;

        public override IEnumerable<NetDatum> Data => _data;

        internal NetIncomingMessage(
            NetMessageType<TBody> type,
            NetSerializer<TBody> serializer, 
            INetDatumProvider dataProvider) : base(type)
        {
            _serializer = serializer;
            _dataProvider = dataProvider;

            this.Type = type;

            _data = new List<NetDatum>();
        }

        public void Read(NetDataReader reader)
        {
            this.ScopeId = reader.GetByte();
            _serializer.Deserialize(reader, _dataProvider, out this.Body);

            while(!reader.EndOfData)
            {
                _data.Add(_dataProvider.Deserialize(reader));
            }
        }

        public override IEnumerator<NetDatum> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override void Recycle()
        {
            _data.Clear();

            this.Type.Recycle(this);
        }

        public INetIncomingMessage Enqueue(INetScopeProvider scopes)
        {
            if(scopes.TryGet(this.ScopeId, out var scope))
            {
                scope.Bus.Publish(typeof(NetIncomingMessage<TBody>), this);
            }

            return this;
        }
    }
}
