using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Security.Dtos;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Services
{
    /// <summary>
    /// Simple manager for users.
    /// </summary>
    public class UserService : Service
    {
        #region Private Fields
        private Dictionary<Int32, User> _users;
        private NetManager _netManager;
        #endregion

        #region Public Properties
        public User this[Int32 id] => _users[id];
        public User this[NetPeer peer] => _users[peer.Id];
        #endregion

        #region Events
        public event OnEventDelegate<UserService, User> OnUserAdded;
        public event OnEventDelegate<UserService, User> OnUserRemoved;
        #endregion

        #region Lifecyele Methods
        protected override void Create(ServiceProvider provider)
        {
            base.Create(provider);

            provider.Service(out _netManager);

            _users = new Dictionary<Int32, User>();
        }

        protected override void Dispose()
        {
            base.Dispose();

            _netManager = default;

            _users.Clear();
        }
        #endregion

        #region Helper Methods
        internal void Remove(Int32 id)
        {
            _users.Remove(id);
        }

        internal User UpdateOrCreate(Int32 id, IEnumerable<Claim> claims)
        {
            if(_users.TryGetValue(id, out User user))
            {
                user.SetClaims(claims);
                return user;
            }

            user = new User(id, claims);
            if(_users.TryAdd(id, user))
            {
                this.OnUserAdded?.Invoke(this, user);
                return user;
            }

            return default;
        }

        internal User UpdateOrCreate(UserDto dto)
        {
            return this.UpdateOrCreate(dto.Id, dto.Claims);
        }
        #endregion
    }
}
