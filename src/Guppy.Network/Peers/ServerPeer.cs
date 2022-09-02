﻿using Guppy.Network.Enums;
using Guppy.Network.Extensions.Identity;
using Guppy.Network.Identity;
using Guppy.Network.Identity.Enums;
using Guppy.Network.Identity.Providers;
using Guppy.Network.Messages;
using Guppy.Network.Providers;
using Guppy.Resources.Providers;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Peers
{
    public class ServerPeer : Peer
    {
        public delegate bool ConnectionApprovalDelegate(ConnectionRequest request, NetIncomingMessage<UserAction> data);

        public event ConnectionApprovalDelegate? ConnectionApproval;

        public ServerPeer(
            ISettingProvider settings,
            INetScopeProvider scopes,
            INetMessageProvider messages,
            IUserProvider users,
            EventBasedNetListener listener,
            NetManager manager,
            NetScope scope) : base(settings, scopes, messages, users, listener, manager, scope)
        {
            this.Authorization = NetAuthorization.Master;

            this.listener.ConnectionRequestEvent += this.HandleConnectionRequestEvent;
            this.listener.PeerDisconnectedEvent += this.HandlePeerDisconnectedEvent;

            this.Users.OnUserConnected += this.HandleUserConnected;
        }

        public void Start(int port)
        {
            base.Start();

            this.manager.Start(port);
        }

        private void HandleUserConnected(IUserProvider sender, User newUser)
        {
            this.Scope.Users.Add(newUser);
        }

        private void HandleConnectionRequestEvent(ConnectionRequest request)
        {
            using (var data = this.messages.Read(request.Data))
            {
                if (data is NetIncomingMessage<UserAction> casted)
                {
                    var accepted = true;

                    if(this.ConnectionApproval is not null)
                    {
                        foreach (ConnectionApprovalDelegate del in this.ConnectionApproval.GetInvocationList())
                        {
                            accepted &= del(request, casted);
                        }
                    }

                    if(accepted)
                    {
                        var peer = request.Accept();

                        var user = this.Users.UpdateOrCreate(peer.Id, peer, casted.Body.Claims);

                        this.Scope.Create(user.CreateAction(UserAction.Actions.CurrentUserConnected, ClaimAccessibility.Protected))
                            .AddRecipient(peer)
                            .Send()
                            .Recycle();

                        return;
                    }
                }

                request.Reject();
            }
        }

        private void HandlePeerDisconnectedEvent(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            this.Users.Remove(peer.Id);
        }
    }
}
