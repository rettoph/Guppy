﻿using Guppy.Network.Security;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GlobalUserCollection : UserCollection
    {
        public GlobalUserCollection() : base(true)
        {

        }

        public User UpdateOrCreate(Guid id, NetIncomingMessage im)
        {
            User user;

            if ((user = this.GetById(id)) == null)
            { // If the user is not defined...
                // create a new one
                user = new User(id, default(Int64));
            }

            user.Read(im);

            return user;
        }
    }
}
