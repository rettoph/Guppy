using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Configurations;
using Guppy.Network.Factories;
using Guppy.Network.Groups;
using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Guppy.Network.Extensions.Lidgren;
using System.Linq;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        #region Private Fields 
        private UserFactory _userFactory;
        private NetServer _server;
        private Dictionary<Int64, User> _approvedUsers;
        #endregion

        #region Constructor
        public ServerPeer(UserFactory userFactory, NetServer server) : base(server)
        {
            _userFactory = userFactory;
            _server = server;
        }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _approvedUsers = new Dictionary<Int64, User>();
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.MessagesTypes.TryAdd(NetIncomingMessageType.ConnectionApproval, this.HandleConnectionApproval);
            this.MessagesTypes.TryAdd(NetIncomingMessageType.StatusChanged, this.HandleStatusChanged);
        }

        public override void Dispose()
        {
            base.Dispose();

            _approvedUsers.Clear();
        }
        #endregion

        #region Peer Implementation
        protected override void SendMessage(NetOutgoingMessageConfiguration omc)
        {
            if(omc.Recipient == default(NetConnection))
            { // Send to the entire group...
                if(omc.Group.connections.Any())
                    _server.SendMessage(omc.Message, omc.Group.connections, omc.Method, omc.SequenceChannel);
            }
            else
            { // Send to the specified recipient...
                _server.SendMessage(omc.Message, omc.Recipient, omc.Method, omc.SequenceChannel);
            }
        }

        /// <inheritdoc />
        protected internal override Type GroupType()
        {
            return typeof(ServerGroup);
        }
        #endregion

        #region Message Handlers
        /// <summary>
        /// Handle incoming connection requests and
        /// approve/deny them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void HandleConnectionApproval(object sender, NetIncomingMessage arg)
        {
            arg.ReadGuid(); // Disregard the user id sent from the client
            var user = _userFactory.Build<User>(u =>
            {
                u.Connection = arg.SenderConnection;
                u.TryRead(arg);
                u.Verified = true;
            });

            var hail = _server.CreateMessage();
            user.TryWrite(hail);
            arg.SenderConnection.Approve(hail);
            _approvedUsers.Add(arg.SenderConnection.RemoteUniqueIdentifier, user);
        }

        private void HandleStatusChanged(object sender, NetIncomingMessage arg)
        {
            switch(arg.SenderConnection.Status) {
                case NetConnectionStatus.Connected:
                    this.Users.Add(_approvedUsers[arg.SenderConnection.RemoteUniqueIdentifier]);
                    _approvedUsers.Remove(arg.SenderConnection.RemoteUniqueIdentifier);
                    break;
                case NetConnectionStatus.Disconnected:
                    this.Users.GetByConnection(arg.SenderConnection).Dispose();
                    break;
            }
        }
        #endregion
    }
}
