using System;
using System.Collections.Generic;
using System.Text;
using Guppy.DependencyInjection;
using Guppy.Network.Groups;
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
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    break;
            }
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
