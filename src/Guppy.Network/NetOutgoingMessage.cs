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
    public class NetOutgoingMessage<TBody> : NetMessage<TBody>, INetOutgoingMessage
    {
        private NetScope _scope;
        private NetDataWriter _writer;
        private readonly NetSerializer<TBody> _serializer;
        private readonly INetDatumProvider _dataProvider;

        private List<NetDatum> _data;
        private List<NetPeer> _recipients;

        public new readonly NetMessageType<TBody> Type;

        public override IEnumerable<NetDatum> Data => _data;
        public NetDataWriter Writer => _writer;

        internal NetOutgoingMessage(
            NetMessageType<TBody> type,
            NetSerializer<TBody> serializer,
            INetDatumProvider dataProvider) : base(type)
        {
            _scope = default!;
            _serializer = serializer;
            _dataProvider = dataProvider;

            this.Type = type;

            _data = new List<NetDatum>();
            _recipients = new List<NetPeer>();
            _writer = new NetDataWriter();

            this.Type.Id.Write(_writer);
        }

        public void Write(in TBody body, NetScope scope)
        {
            _scope = scope;
            this.ScopeId = scope.id;

            this.Body = body;

            _writer.Put(scope.id);
            _serializer.Serialize(_writer, in body);
        }

        public override IEnumerator<NetDatum> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override void Recycle()
        {
            _writer.SetPosition(NetId.SizeInBytes);

            foreach(NetDatum datum in _data)
            {
                datum.Recycle();
            }

            _data.Clear();
            _recipients.Clear();

            this.Type.Recycle(this);
        }

        public INetOutgoingMessage Append<TValue>(in TValue value)
        {
            _data.Add(_dataProvider.Serialize(_writer, in value));

            return this;
        }

        public INetOutgoingMessage AddRecipient(NetPeer recipient)
        {
            _recipients.Add(recipient);

            return this;
        }

        public INetOutgoingMessage AddRecipients(IEnumerable<NetPeer> recipients)
        {
            _recipients.AddRange(recipients);

            return this;
        }

        public INetOutgoingMessage Send()
        {
            foreach (NetPeer recipient in _recipients)
            {
                recipient.Send(_writer, this.Type.OutgoingChannel, this.Type.DeliveryMethod);
            }

            return this;
        }

        public INetOutgoingMessage Enqueue()
        {
            _scope.Enqueue(this);

            return this;
        }
    }
}
