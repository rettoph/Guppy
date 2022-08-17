using Guppy.Network.Providers;
using Guppy.Network.Structs;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class NetIncomingMessage<THeader> : NetMessage<THeader>, INetIncomingMessage
    {
        private readonly NetSerializer<THeader> _serializer;
        private readonly INetDatumProvider _dataProvider;
        private List<NetDatum> _data;

        public new readonly NetMessageType<THeader> Type;

        public override IEnumerable<NetDatum> Data => _data;

        internal NetIncomingMessage(
            NetMessageType<THeader> type,
            NetSerializer<THeader> serializer, 
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
            _serializer.Deserialize(reader, out this.Header);

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
    }
}
