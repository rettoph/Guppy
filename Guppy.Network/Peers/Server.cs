using Guppy.Network.Security;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Guppy.Network.Security.Interfaces;
using Lidgren.Network;
using Microsoft.Extensions.Logging;

namespace Guppy.Network.Peers
{
    public class Server : Peer
    {
        #region protected Attributes
        protected NetServer server;
        #endregion

        #region Public Attributes
        public IAuthenticationService AuthenticationService { get; private set; }
        #endregion

        #region Constructors
        public Server(NetPeerConfiguration config, ILogger logger) : base(config, logger)
        {
            this.server = new NetServer(this.config);
            this.peer = this.server;
        }
        #endregion
    }
}
