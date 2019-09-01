using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Factories;
using Guppy.Network.Security;
using Lidgren.Network;

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

            this.Messages.TryAdd(NetIncomingMessageType.ConnectionApproval, this.HandleConnectionApproval);
            this.Messages.TryAdd(NetIncomingMessageType.StatusChanged, this.HandleStatusChanged);
        }

        public override void Dispose()
        {
            base.Dispose();

            _approvedUsers.Clear();
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
            var user = _userFactory.Build<User>(u =>
            {
                u.Connection = arg.SenderConnection;
                u.Read(arg.SenderConnection.RemoteHailMessage);
            });

            var hail = _server.CreateMessage();
            user.Write(hail);
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
            }
        }
        #endregion
    }
}
