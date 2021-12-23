using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.EventArgs;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.Lists;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network
{
    public class Pipe : Entity
    {
        #region Private Fields
        private Dictionary<UInt16, INetworkEntity> _entities;
        #endregion

        #region Public Properties
        public Room Room { get; internal set; }
        public UserList Users { get; private set; }
        public IEnumerable<INetworkEntity> NetworkEntities => _entities.Values;
        #endregion

        #region Events
        public event OnEventDelegate<Pipe, NetworkEntityPipeEventArgs> OnNetworkEntityAdded;
        public event OnEventDelegate<Pipe, NetworkEntityPipeEventArgs> OnNetworkEntityRemoved;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<UserList>();

            _entities = new Dictionary<UInt16, INetworkEntity>();
        }

        protected override void PostRelease()
        {
            base.PostRelease();

            _entities.Clear();

            this.Users.TryRelease();
            this.Users = default;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Send a message within the current room to a specific recipient
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TData>(TData data, NetPeer reciepient)
            where TData : class, IData
        {
            this.Room.SendMessage(data, reciepient);
        }

        /// <summary>
        /// Send a message within the current room to a specific collection of recipients
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TData>(TData data, IEnumerable<NetPeer> reciepients)
            where TData : class, IData
        {
            this.Room.SendMessage(data, reciepients);
        }

        /// <summary>
        /// Send a message within the current room to all joined peers
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TData>(TData data)
            where TData : class, IData
        {
            this.Room.SendMessage(data, this.Users.NetPeers);
        }

        internal Boolean TryAddNetworkEntity(INetworkEntity entity, Pipe oldPipe)
        {
            if(_entities.TryAdd(entity.NetworkId, entity))
            {
                this.OnNetworkEntityAdded?.Invoke(this, new NetworkEntityPipeEventArgs(this, oldPipe, entity));
                return true;
            }

            return false;
        }

        internal void RemoveNetworkEntity(INetworkEntity entity, Pipe newPipe)
        {
            _entities.Remove(entity.NetworkId);
            this.OnNetworkEntityRemoved?.Invoke(this, new NetworkEntityPipeEventArgs(newPipe, this, entity));
        }
        #endregion
    }
}
