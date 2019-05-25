using Guppy.Network.Collections;
using Guppy.Network.Groups;
using Guppy.Network.Interfaces;
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
            g.Update();

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
                // 
            });

            var serverDriver = new ServerNetworkSceneDriver();
            NetworkSceneDriver.DefaultServer = new NetworkSceneDriver(serverDriver.Setup, serverDriver.Update);
        }
    }

    class ServerNetworkSceneDriver
    {
        private Queue<NetworkEntity> _dirtyEntityQueue;
        private Queue<NetOutgoingMessage> _createdMessageQueue;

        public ServerNetworkSceneDriver()
        {
            _dirtyEntityQueue = new Queue<NetworkEntity>();
            _createdMessageQueue = new Queue<NetOutgoingMessage>();
        }

        public void Setup(NetworkScene s, Group g, NetworkEntityCollection ne)
        {
            ne.Created += this.HandleNetworkEntityCreated;
            ne.Removed += this.HandleNetworkEntityRemoved;
        }

        public void Update(NetworkScene s, Group g, NetworkEntityCollection ne)
        {
            if (g.Users.Count() > 0)
            {
                while (_createdMessageQueue.Count > 0)
                    g.SendMesssage(_createdMessageQueue.Dequeue(), NetDeliveryMethod.ReliableOrdered);

                while (_dirtyEntityQueue.Count > 0)
                    g.SendMesssage(_dirtyEntityQueue.Dequeue().BuildUpdateMessage(), NetDeliveryMethod.ReliableSequenced);
            }
            else
            {
                _createdMessageQueue.Clear();
                _dirtyEntityQueue.Clear();
            }
        }

        private void HandleNetworkEntityCreated(object sender, NetworkEntity e)
        {
            _createdMessageQueue.Enqueue(e.BuildCreateMessage());

            e.OnDirtyChanged += this.HandleNetworkEntityDirtyChanged;
        }

        private void HandleNetworkEntityRemoved(object sender, NetworkEntity e)
        {
            e.OnDirtyChanged -= this.HandleNetworkEntityDirtyChanged;
        }

        private void HandleNetworkEntityDirtyChanged(object sender, ITrackedNetworkObject e)
        {
            if (e.Dirty)
                _dirtyEntityQueue.Enqueue(e as NetworkEntity);
        }
    }
}
