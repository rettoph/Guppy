using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.Network.Security.Dtos;
using Guppy.Network.Security.Lists;
using Guppy.Network.Security.Structs;
using LiteNetLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security.Services
{
    /// <summary>
    /// Simple manager for users.
    /// </summary>
    public class UserService : UserList
    {
        #region Private Fields
        private NetManager _netManager;
        #endregion

        #region Lifecyele Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            provider.Service(out _netManager);
        }
        #endregion

        #region Helper Methods
        internal User UpdateOrCreate(Int32 id, IEnumerable<Claim> claims)
        {
            lock (this)
            {
                if (this.TryGetById(id, out User user))
                {
                    user.SetClaims(claims);
                    return user;
                }

                user = new User(id, _netManager, claims);
                if (this.TryAdd(user))
                {
                    return user;
                }

                throw new InvalidOperationException();
            }
        }

        internal User UpdateOrCreate(UserDto dto)
        {
            return this.UpdateOrCreate(dto.Id, dto.Claims);
        }
        #endregion
    }
}
