using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Guppy.Network
{
    public abstract class NetworkGame : Game
    {
        #region Private Attributes
        private Peer _peer;
        #endregion

        public NetworkGame(int seed = 1337) : base(seed)
        {
        }

        #region Initializeation Methods
        protected override void Boot()
        {
            base.Boot();

            this.services.AddSingleton<Peer>(this.PeerFactory);
            this.services.AddSingleton<Client>(this.GetPeer<Client>);
            this.services.AddSingleton<Server>(this.GetPeer<Server>);
        }

        private Server GetPeer<TPeer>()
        {
            throw new NotImplementedException();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            // Start the game's peer
            _peer = this.provider.GetService<Peer>();
            _peer.Start();
        }
        #endregion

        #region PeerGame Methods
        protected abstract Peer PeerFactory(IServiceProvider arg);
        private TPeer GetPeer<TPeer>(IServiceProvider arg)
            where TPeer : Peer
        {
            if (_peer is TPeer)
                return _peer as TPeer;
            else
                throw new Exception($"Unable to cast current Peer instance to {typeof(TPeer).Name}");
        }
        #endregion
    }
}
