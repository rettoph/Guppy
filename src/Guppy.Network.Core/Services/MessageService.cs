using Guppy.Network.Configurations;
using Guppy.Network.Enums;
using Guppy.Network.Interfaces;
using Guppy.Utilities;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Services
{
    public class MessageService : DataService<MessageConfiguration>
    {
        private Factory<Message> _messages;
        private PacketService _packets;

        internal MessageService(DynamicIdSize idSize, MessageConfiguration[] configurations) : base(idSize, configurations)
        {
            _messages = new Factory<Message>(() => new Message());
        }

        public void SendMessage(UInt16 channelId, IPacket data, IEnumerable<IPacket> packets)
        {
            Message message =_messages.Create();

            message.ChannelId = channelId;
            message.Configuration = this.GetConfiguration(data);
            message.Data = data;
            message.Packets.AddRange(packets);
        }

        private void WriteMessage(NetDataWriter om, Message message)
        {
            om.Put(message.ChannelId);
            this.WriteConfiguration(om, message.Configuration);

            message.Configuration.DataWriter(om, message.Data);

            foreach(IPacket packet in message.Packets)
            {
                PacketConfiguration pConfiguration = _packets.GetConfiguration(packet);

                _packets.WriteConfiguration(om, pConfiguration);
                pConfiguration.DataWriter(om, packet);
            }
        }

        private void ReadMessage(NetDataReader im)
        {
            Message message = _messages.Create();

            message.ChannelId = im.GetByte();
            message.Configuration = this.ReadConfiguration(im);
            message.Data = message.Configuration.DataReader(im);

            while(!im.EndOfData)
            {
                PacketConfiguration pConfiguration = _packets.ReadConfiguration(im);
                message.Packets.Add(pConfiguration.DataReader(im));
            }
        }
    }
}
