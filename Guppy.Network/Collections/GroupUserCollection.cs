using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GroupUserCollection : NetworkObjectCollection<User>
    {
        public GroupUserCollection() : base(false)
        {

        }

        public User GetByClaim(String key, String value)
        {
            return this.list.FirstOrDefault(u => u.Get(key) == value);
        }
        public User GetByNetConnection(NetConnection connection)
        {
            return this.GetByClaim("connection", connection.RemoteUniqueIdentifier.ToString());
        }
    }
}
