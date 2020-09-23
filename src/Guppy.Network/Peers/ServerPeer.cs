using Guppy.DependencyInjection;
using Guppy.Extensions.Collections;
using Guppy.Network.Groups;
using Guppy.Network.Extensions.Lidgren;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Collections;
using Guppy.Network.Utilities;
using Guppy.Network.Structs;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.Network.Peers
{
    public sealed class ServerPeer : Peer
    {
        #region Private Fields
        private NetServer _server;
        private UserNetConnectionDictionary _userConnections;
        #endregion

        #region Events
        public delegate Boolean AuthenticateUserDelegate(User user, NetConnection connection);

        /// <summary>
        /// Invoked when a user must be authenticated before
        /// connection approval. Custom authentication methods
        /// can just be added here. 
        /// 
        /// True: Authenticated
        /// False: Denied
        /// </summary>
        public event AuthenticateUserDelegate AuthenticateUser;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            provider.Service<NetServer>(out _server);
            provider.Service<UserNetConnectionDictionary>(out _userConnections);

            base.PreInitialize(provider);
        }

        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            this.MessageTypeDelegates[NetIncomingMessageType.StatusChanged] += this.HandleSatusChangedMessageType;
        }

        protected override void Release()
        {
            base.Release();

            this.MessageTypeDelegates[NetIncomingMessageType.StatusChanged] -= this.HandleSatusChangedMessageType;
        }
        #endregion

        #region Helper Methods
        protected override void Start(bool draw)
        {
            base.Start(draw);

            // Create a new default user representing the server...
            this.CurrentUser = this.provider.GetService<User>((u, p, c) =>
            { // Create a new user instance...
                u.Name = "Server";
            });
            this.Users.TryAdd(this.CurrentUser);
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
                    this.HandleRespondedAwaitingApprovalSatusChangedMessageType(im);
                    break;
                case NetConnectionStatus.RespondedConnect:
                    break;
                case NetConnectionStatus.Connected:
                    this.HandleConnectedSatusChangedMessageType(im);
                    break;
                case NetConnectionStatus.Disconnecting:
                    break;
                case NetConnectionStatus.Disconnected:
                    this.HandleDisconnectedSatusChangedMessageType(im);
                    break;
            }
        }
        #endregion

        #region StatusChanged MessageType Handlers
        /// <summary>
        /// internal method used to approve or deny 
        /// incoming connection requests.
        /// </summary>
        /// <param name="im"></param>
        private void HandleRespondedAwaitingApprovalSatusChangedMessageType(NetIncomingMessage im)
        {
            // When a new incoming request is recieved...
            var user = this.provider.GetService<User>((u, p, c) =>
            { // Create a new user instance...
                u.TryRead(im.SenderConnection.RemoteHailMessage);
            });

            Boolean valid = true;
            if (this.AuthenticateUser != null)
            { // If there is no custom authentication, auto approve...
                foreach (AuthenticateUserDelegate authentication in this.AuthenticateUser.GetInvocationList())
                { // Iterate through each authentication methods...
                    if (!authentication(user, im.SenderConnection))
                    { // If any of the methods fail...
                        valid = false;
                        break;
                    }
                }
            }

            // Now that all authentication methods have been checked, approve of deny the connection
            if (valid)
            {
                // Save the connection user combo for 2 way lookup.
                _userConnections.Add(user, im.SenderConnection);

                // Approve the message complete with a hail user instance.
                var hail = _server.CreateMessage();
                hail.Write(user.Id);
                user.TryWrite(hail);
                im.SenderConnection.Approve(hail);
            }
            else
                im.SenderConnection.Deny();
        }

        /// <summary>
        /// Automatically add the new user into the server's global 
        /// user collection on a new connection.
        /// </summary>
        /// <param name="im"></param>
        private void HandleConnectedSatusChangedMessageType(NetIncomingMessage im)
        {
            this.Users.TryAdd(_userConnections.Users[im.SenderConnection]);
        }

        /// <summary>
        /// Automatically dispose of a user instance when the connection
        /// is lose.
        /// </summary>
        /// <param name="im"></param>
        private void HandleDisconnectedSatusChangedMessageType(NetIncomingMessage im)
        {
            // Automatically dispose of the old user.
            _userConnections.Users[im.SenderConnection].TryRelease();
            _userConnections.Remove(im.SenderConnection);
        }
        #endregion

        #region Messageable Implementation
        protected override void Send(NetOutgoingMessageConfiguration message)
        {
            if (message.Recipient != default(NetConnection))
                _server.SendMessage(
                    msg: message.Message,
                    recipient: message.Recipient,
                    method: message.Method,
                    sequenceChannel: message.SequenceChannel);
            else
                _server.SendToAll(
                    msg: message.Message,
                    except: default(NetConnection),
                    method: message.Method,
                    sequenceChannel: message.SequenceChannel);
        }
        #endregion

        #region Peer Implementation
        protected override NetPeer GetPeer(ServiceProvider provider)
            => _server;

        internal override Group GroupFactory()
            => new ServerGroup();
        #endregion
    }
}
