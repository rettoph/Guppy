using Guppy.Network.Collections;
using Guppy.Network.Groups;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network
{
    public class NetworkSceneDriver
    {
        public static NetworkSceneDriver DefaultClient { get; private set; }
        public static NetworkSceneDriver DefaultServer { get; private set; }

        public Action<NetworkScene, Group, NetworkEntityCollection> Update;
        public Action<NetworkScene> Setup;

        private NetworkSceneDriver(Action<NetworkScene> setup, Action<NetworkScene, Group, NetworkEntityCollection> update)
        {
            this.Setup = setup;
            this.Update = update;
        }

        static NetworkSceneDriver()
        {
            NetworkSceneDriver.DefaultClient = new NetworkSceneDriver(s =>
            {
                // 
            },
            (s, g, ne) =>
            {
                while (ne.CreatedQueue.Count > 0)
                    g.SendMesssage(ne.CreatedQueue.Dequeue().BuildCreateMessage(g), NetDeliveryMethod.ReliableSequenced, 1);

                while (ne.DirtyQueue.Count > 0)
                    g.SendMesssage(ne.DirtyQueue.Dequeue().BuildUpdateMessage(g), NetDeliveryMethod.ReliableSequenced, 1);
            });

            NetworkSceneDriver.DefaultServer = new NetworkSceneDriver(s =>
            {
                // 
            },
            (s, g, ne) =>
            {
                while (ne.DirtyQueue.Count > 0)
                    g.SendMesssage(ne.DirtyQueue.Dequeue().BuildUpdateMessage(g), NetDeliveryMethod.UnreliableSequenced, 1);
            });
        }
    }
}
