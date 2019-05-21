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

        public Action<NetworkScene, Group, NetworkEntityCollection> update;
        public Action<NetworkScene, Group, NetworkEntityCollection> setup;

        public NetworkSceneDriver(
            Action<NetworkScene, Group, NetworkEntityCollection> setup, 
            Action<NetworkScene, Group, NetworkEntityCollection> update)
        {
            this.setup = setup;
            this.update = update;
        }

        public void Update(NetworkScene s, Group g, NetworkEntityCollection ne)
        {
            this.update(s, g, ne);

            if (g.Users.Count() > 0)
            {
                // All drivers should auto-push any recieved actions
                while (s.actionQueue.Count > 0)
                    g.SendMesssage(s.actionQueue.Dequeue(), NetDeliveryMethod.UnreliableSequenced);
            }
            else
            {
                s.actionQueue.Clear();
            }
        }
        public void Setup(NetworkScene s, Group g, NetworkEntityCollection ne)
        {
            this.setup(s, g, ne);
        }

        static NetworkSceneDriver()
        {
            NetworkSceneDriver.DefaultClient = new NetworkSceneDriver((s, g, ne) =>
            {
                // 
            },
            (s, g, ne) =>
            {
                g.Update();
                while (ne.DirtyQueue.Count > 0)
                    g.SendMesssage(ne.DirtyQueue.Dequeue().BuildUpdateMessage(), NetDeliveryMethod.ReliableSequenced);
            });

            var serverDriver = new ServerNetworkSceneDriver();
            NetworkSceneDriver.DefaultServer = new NetworkSceneDriver(serverDriver.Setup, serverDriver.Update);
        }
    }

    class ServerNetworkSceneDriver
    {
        private Queue<NetOutgoingMessage> _createdMessageQueue;

        public ServerNetworkSceneDriver()
        {
            _createdMessageQueue = new Queue<NetOutgoingMessage>();
        }

        public void Setup(NetworkScene s, Group g, NetworkEntityCollection ne)
        {
            ne.Created += this.HandleNetworkEntityCreated;
        }

        public void Update(NetworkScene s, Group g, NetworkEntityCollection ne)
        {
            g.Update();

            if (g.Users.Count() > 0)
            {
                while (_createdMessageQueue.Count > 0)
                    g.SendMesssage(_createdMessageQueue.Dequeue(), NetDeliveryMethod.ReliableOrdered);

                while (ne.DirtyQueue.Count > 0)
                    g.SendMesssage(ne.DirtyQueue.Dequeue().BuildUpdateMessage(), NetDeliveryMethod.ReliableSequenced);
            }
            else
            {
                _createdMessageQueue.Clear();
                ne.DirtyQueue.Clear();
            }
        }

        private void HandleNetworkEntityCreated(object sender, NetworkEntity e)
        {
            _createdMessageQueue.Enqueue(e.BuildCreateMessage());
        }
    }
}
