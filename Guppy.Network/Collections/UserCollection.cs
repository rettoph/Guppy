﻿using Guppy.Collections;
using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Guppy.Network.Collections
{
    public class UserCollection : UniqueObjectCollection<User>
    {
        private Dictionary<Int64, User> _netIdTable;

        public UserCollection(Boolean disposeOnRemove = false) : base(disposeOnRemove)
        {
            _netIdTable = new Dictionary<Int64, User>();
        }

        public override void Add(User item)
        {
            base.Add(item);

            _netIdTable.Add(item.NetId, item);
        }

        public override bool Remove(User item)
        {
            if(base.Remove(item))
            {
                _netIdTable.Remove(item.NetId);

                return true;
            }

            return false;
        }

        public User GetByClaim(String key, String value)
        {
            return this.list.FirstOrDefault(u => u.Get(key) == value);
        }
        public User GetByNetConnection(NetConnection connection)
        {
            return this.GetByNetId(connection.RemoteUniqueIdentifier);
        }
        public User GetByNetId(Int64 netId)
        {
            return _netIdTable[netId];
        }
    }
}
