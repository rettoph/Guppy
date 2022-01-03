using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Enums;
using Guppy.Network.EventArgs;
using Guppy.Network.Security.Enums;
using Guppy.Network.Security.EventArgs;
using Guppy.Threading.Interfaces;
using Guppy.Threading.Utilities;
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
        public event OnEventDelegate<UserList, UserListEventArgs> OnEvent;

        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _users = new Dictionary<Int32, User>();
            _netPeers = new Dictionary<Int32, NetPeer>();
        }

        protected override void Uninitialize()
        {
            base.Uninitialize();

            while(_users.Any())
            {
                this.TryRemove(_users[0]);
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Immidiately add a new user into the <see cref="UserList"/>
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Boolean TryAdd(User user)
        {
            if (_users.TryAdd(user.Id, user))
            {
                user.OnDisposing += this.HandleUserDisposing;

                if (user.NetPeer is null || _netPeers.TryAdd(user.NetPeer.Id, user.NetPeer))
                {
                    this.OnEvent?.Invoke(this, new UserListEventArgs(user, UserListAction.Added));
                    return true;
                }
                else
                {
                    user.OnDisposing -= this.HandleUserDisposing;
                }
            }

            return false;
        }

        /// <summary>
        /// Immidiately remove a user from the <see cref="UserList"/>
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual Boolean TryRemove(User user)
        {
            if (_users.ContainsKey(user.Id))
            {
                _users.Remove(user.Id);
                user.OnDisposing -= this.HandleUserDisposing;

                if (user.NetPeer is not null)
                {
                    _netPeers.Remove(user.NetPeer.Id);
                }

                this.OnEvent?.Invoke(this, new UserListEventArgs(user, UserListAction.Removed));
                return true;
            }

            return false;
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
