using Guppy.Network.Groups;
using Guppy.Network.Peers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Configurations
{
    public class NetworkConfiguration
    {
        public static NetworkConfiguration Server { get; private set; }
        public static NetworkConfiguration Client { get; private set; }


        public readonly Type Group;
        public readonly Type Peer;

        public NetworkConfiguration(Type peer, Type group)
        {
            this.Peer = peer;
            this.Group = group;
        }

        static NetworkConfiguration()
        {
            NetworkConfiguration.Server = new NetworkConfiguration(typeof(ServerPeer), typeof(ServerGroup));
            NetworkConfiguration.Client = new NetworkConfiguration(typeof(ClientPeer), typeof(ClientGroup));
        }
    }
}
