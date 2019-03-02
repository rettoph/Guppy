using Guppy.Network.Peers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public abstract class PeerGame : Game
    {
        #region Private Attributes
        private Peer _peer;
        #endregion

        public PeerGame(int seed = 1337) : base(seed)
        {
        }

        #region Initializeation Methods
        protected override void Boot()
        {
            base.Boot();


            _peer = this.BuildPeer();
            this.services.AddSingleton<Peer>(_peer);

            if (_peer is Client)
                this.services.AddSingleton<Client>(_peer as Client);
            if (_peer is Server)
                this.services.AddSingleton<Server>(_peer as Server);
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();

            // Start the game's peer
            _peer.Start();
        }
        #endregion

        #region PeerGame Methods
        /// <summary>
        /// Build an instance of a peer object. This will automatically be called once
        /// and be added to the service collection.
        /// </summary>
        /// <returns>The games peer (added as a singleton)</returns>
        protected internal abstract Peer BuildPeer();
        #endregion
    }
}
