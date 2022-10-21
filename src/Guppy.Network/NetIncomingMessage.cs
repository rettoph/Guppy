﻿using Guppy.Common;
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
    public class NetIncomingMessage<T> : INetIncomingMessage<T>
        where T : notnull
    {
        private readonly NetScope _scope;
        private readonly NetMessageType<T> _type;
        private readonly INetSerializerProvider _serializers;
        private readonly INetSerializer<T> _serializer;
        private T _body;
        private IList<object> _data;

        public IEnumerable<object> Data => _data;

        T INetIncomingMessage<T>.Body => _body;

        NetMessageType<T> INetIncomingMessage<T>.Type => _type;

        object INetIncomingMessage.Body => _body;

        NetMessageType INetIncomingMessage.Type => _type;

        Type IMessage.PublishType { get; } = typeof(INetIncomingMessage<T>);

        internal NetIncomingMessage(
            NetMessageType<T> type,
            NetScope scope,
            INetSerializerProvider serializers)
        {
            _body = default!;
            _type = type;
            _scope = scope;
            _serializers = serializers;
            _serializer = _serializers.Get<T>();

            _data = new List<object>();
        }

        public void Read(NetDataReader reader)
        {
            _body = _serializer.Deserialize(reader, _serializers);

            while(!reader.EndOfData)
            {
                _data.Add(_serializers.Deserialize(reader));
            }
        }

        public void Recycle()
        {
            _data.Clear();

            _type.Recycle(this);
        }

        public INetIncomingMessage<T> Enqueue()
        {
            _scope.Bus.Publish(this);

            return this;
        }

        INetIncomingMessage INetIncomingMessage.Enqueue()
        {
            return this.Enqueue();
        }

        void IDisposable.Dispose()
        {
            this.Recycle();
        }
    }
}
