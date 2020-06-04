using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using Guppy.Network.Enums;
using Guppy.Network.Extensions.Lidgren;
using Guppy.Network.Groups;
using Guppy.Network.Structs;
using Lidgren.Network;

namespace Guppy.Network.Peers
{
    public sealed class ClientPeer : Peer
    {
        #region Private Fields
        private NetClient _client;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            provider.Service<NetClient>(out _client);
            
            base.PreInitialize(provider);

            this.MessageTypeDelegates[NetIncomingMessageType.StatusChanged] += this.HandleSatusChangedMessageType;
        }
        #endregion

        #region Connect Methods
        public void TryConnect(String host, Int32 port, User user)
        {
            if(_client.ConnectionStatus == NetConnectionStatus.Disconnected)
            {
                var hail = _client.CreateMessage();
                user.TryWrite(hail);

                _client.Connect(host, port, hail);
            }
                
        }
        #endregion

        #region MessageType Handlers
        private void HandleSatusChangedMessageType(NetIncomingMessage im)
        {
            im.Position = 0;
            switch ((NetConnectionStatus)im.ReadByte())
            {
                case NetConnectionStatus.None:
                    break;
                case NetConnectionStatus.InitiatedConnect:
                    break;
                case NetConnectionStatus.ReceivedInitiation:
                    break;
                case NetConnectionStatus.RespondedAwaitingApproval:
                    break;
                case NetConnectionStatus.RespondedConnect:
                    break;
                case NetConnectionStatus.Connected:
                    this.HandleConnectedSatusChangedMessageType(im);
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    break;
            }
        }
        #endregion

        #region StatucChangedMessageType Handlers
        private void HandleConnectedSatusChangedMessageType(NetIncomingMessage im)
        {
            // Read the new user instance
            this.CurrentUser = this.provider.GetService<User>((u, p, c) =>
            {
                u.Id = im.SenderConnection.RemoteHailMessage.ReadGuid();
                u.TryRead(im.SenderConnection.RemoteHailMessage);
            });

            // Add the recieved user into the global user collection...
            this.Users.TryAdd(this.CurrentUser);
        }
        #endregion

        #region Messageable Implementation
        protected override void Send(NetOutgoingMessageConfiguration message)
        {
            _client.SendMessage(
                msg: message.Message,
                method: message.Method,
                sequenceChannel: message.SequenceChannel);
        }
        #endregion

        #region Peer Implementation
        protected override NetPeer GetPeer(ServiceProvider provider)
            => _client;

        internal override Group GroupFactory()
            => new ClientGroup();
        #endregion
    }
}
