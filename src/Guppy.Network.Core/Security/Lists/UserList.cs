using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.EventArgs;
using Guppy.Network.Security.EventArgs;
using LiteNetLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Security.Lists
{
    public class UserList : Service, IEnumerable<User>
    {
        #region Private Fields
        private Dictionary<Int32, User> _users;
        private Dictionary<Int32, NetPeer> _netPeers;
        #endregion

        #region Public Properties
        public IEnumerable<NetPeer> NetPeers => _netPeers.Values;

        public User this[Int32 id] => _users[id]; 
        #endregion

        #region Events
        public event OnEventDelegate<UserList, UserEventArgs> OnUserAdded;
        public event OnEventDelegate<UserList, UserEventArgs> OnUserRemoved;

        public event OnEventDelegate<UserList, NetPeerEventArgs> OnNetPeerAdded;
        public event OnEventDelegate<UserList, NetPeerEventArgs> OnNetPeerRemoved;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _users = new Dictionary<Int32, User>();
            _netPeers = new Dictionary<Int32, NetPeer>();
        }

        protected override void Release()
        {
            base.Release();

            while(_users.Any())
            {
                this.TryRemove(_users[0]);
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Attempt to add a user into the list...
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Boolean TryAdd(User user)
        {
            if(_users.TryAdd(user.Id, user))
            {
                if(user.NetPeer is null)
                {
                    user.OnDisposing += this.HandleUserDisposing;
                    this.OnUserAdded?.Invoke(this, new UserEventArgs(user));
                    return true;
                }
                else if(_netPeers.TryAdd(user.NetPeer.Id, user.NetPeer))
                {
                    user.OnDisposing += this.HandleUserDisposing;
                    this.OnUserAdded?.Invoke(this, new UserEventArgs(user));
                    this.OnNetPeerAdded?.Invoke(this, new NetPeerEventArgs(user.NetPeer));
                    return true;
                }
                else
                {
                    _users.Remove(user.Id);
                }
            }

            return false;
        }

        /// <summary>
        /// Remove a user from the list...
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Boolean TryRemove(User user)
        {
            _users.Remove(user.Id);
            user.OnDisposing -= this.HandleUserDisposing;

            this.OnUserRemoved?.Invoke(this, new UserEventArgs(user));

            if (user.NetPeer is not null)
            {
                _netPeers.Remove(user.NetPeer.Id);
                this.OnNetPeerRemoved?.Invoke(this, new NetPeerEventArgs(user.NetPeer));
            }            

            return true;
        }

        /// <summary>
        /// Attempt to get a user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public Boolean TryGetById(Int32 id, out User user)
        {
            return _users.TryGetValue(id, out user);
        }
        #endregion

        #region IEnumerable<User> Implementation
        public IEnumerator<User> GetEnumerator()
        {
            return _users.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region Event Handlers
        private void HandleUserDisposing(User user)
        {
            this.TryRemove(user);
        }
        #endregion
    }
}
