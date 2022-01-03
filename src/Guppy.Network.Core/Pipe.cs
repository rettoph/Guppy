using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.EventArgs;
using Guppy.Network.Interfaces;
using Guppy.Network.Security.Lists;
using Guppy.Threading.Interfaces;
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
        private Dictionary<UInt16, IMagicNetworkEntity> _entities;
        #endregion

        #region Public Properties
        public Room Room { get; internal set; }
        public UserList Users { get; private set; }
        public IEnumerable<IMagicNetworkEntity> NetworkEntities => _entities.Values;
        #endregion

        #region Events
        public event OnEventDelegate<Pipe, MagicNetworkEntityPipeEventArgs> OnNetworkEntityAdded;
        public event OnEventDelegate<Pipe, MagicNetworkEntityPipeEventArgs> OnNetworkEntityRemoved;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.Users = provider.GetService<UserList>();

            _entities = new Dictionary<UInt16, IMagicNetworkEntity>();
        }

        protected override void PostUninitialize()
        {
            base.PostUninitialize();

            _entities.Clear();

            this.Users.Dispose();
        }
        #endregion

        #region Helper Methods
        internal void SetId(Guid id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Send a message within the current room to a specific recipient
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TMessage>(TMessage data, NetPeer reciepient)
            where TMessage : class, IData
        {
            this.Room.SendMessage(data, reciepient);
        }

        /// <summary>
        /// Send a message within the current room to a specific collection of recipients
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TMessage>(TMessage data, IEnumerable<NetPeer> reciepients)
            where TMessage : class, IData
        {
            this.Room.SendMessage(data, reciepients);
        }

        /// <summary>
        /// Send a message within the current room to all joined peers
        /// </summary>
        /// <param name="data"></param>
        public void SendMessage<TMessage>(TMessage data)
            where TMessage : class, IData
        {
            this.Room.SendMessage(data, this.Users.NetPeers);
        }

        internal Boolean TryAddNetworkEntity(IMagicNetworkEntity entity, Pipe oldPipe)
        {
            if(_entities.TryAdd(entity.NetworkId, entity))
            {
                this.OnNetworkEntityAdded?.Invoke(this, new MagicNetworkEntityPipeEventArgs(this, oldPipe, entity));
                return true;
            }

            return false;
        }

        internal void RemoveNetworkEntity(IMagicNetworkEntity entity, Pipe newPipe)
        {
            _entities.Remove(entity.NetworkId);
            this.OnNetworkEntityRemoved?.Invoke(this, new MagicNetworkEntityPipeEventArgs(newPipe, this, entity));
        }
        #endregion
    }
}
