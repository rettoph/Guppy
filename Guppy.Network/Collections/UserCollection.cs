using Guppy.Collections;
using Guppy.Network.Security.Authentication;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Collections
{
    public class UserCollection : UniqueCollection<User>
    {
        public UserCollection(IServiceProvider provider) : base(provider)
        {
        }

        #region Helper Methods
        public User GetByNetConnection(NetConnection connection)
        {
            return this.First(u => u.NetConnection == connection);
        }
        #endregion
    }
}
