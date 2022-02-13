using Guppy.Network.Configurations;
using Guppy.Threading.Interfaces;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Utilities
{
    public class OutgoingMessage
    {
        private Room _room;
        private NetworkProvider _network;

        private NetDataWriter _writer;
        private NetworkMessageConfiguration _configuration;
        private List<NetPeer> _reciepients;

        public Int32 Priority => _configuration.OutgoingPriority;

        internal OutgoingMessage(Room room, NetworkProvider network)
        {
            _room = room;
            _network = network;

            _reciepients = new List<NetPeer>();
        }

        public void Prepare<TData>(TData message, NetPeer recipient)
            where TData : IData
        {
            _reciepients.Add(recipient);
            _network.WriteMessage(_room, message, out _writer, out _configuration);
            _room.Messages.TryEnqueueOutgoingMessage(this);
        }

        public void Prepare<TData>(ref TData message, NetPeer recipient)
                where TData : struct, IData
        {
            _reciepients.Add(recipient);
            _network.WriteMessage(_room, message, out _writer, out _configuration);
            _room.Messages.TryEnqueueOutgoingMessage(this);
        }

        public void Prepare<TData>(TData message, IEnumerable<NetPeer> recipients)
            where TData : IData
        {
            _reciepients.AddRange(recipients);
            _network.WriteMessage(_room, message, out _writer, out _configuration);
            _room.Messages.TryEnqueueOutgoingMessage(this);
        }

        public void Prepare<TData>(ref TData message, IEnumerable<NetPeer> recipients)
                where TData : struct, IData
        {
            _reciepients.AddRange(recipients);
            _network.WriteMessage(_room, message, out _writer, out _configuration);
            _room.Messages.TryEnqueueOutgoingMessage(this);
        }

        public void Send(ref Int32 counter)
        {
            foreach (NetPeer recipient in _reciepients)
            {
                recipient.Send(_writer, _configuration.SequenceChannel, _configuration.DeliveryMethod);
                counter++;
            }

            _network.RecycleMessage(_writer);
            _reciepients.Clear();
        }
    }
}
