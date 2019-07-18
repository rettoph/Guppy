using Guppy.Network.Security;
using Lidgren.Network;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GlobalUserCollection : UserCollection
    {
        private IServiceProvider _provider;

        public GlobalUserCollection(IServiceProvider provider) : base(true)
        {
            _provider = provider;
        }

        public User UpdateOrCreate(Guid id, NetIncomingMessage im)
        {
            User user;

            if ((user = this.GetById(id)) == null)
            { // If the user is not defined...
                // create a new one
                user = ActivatorUtilities.CreateInstance<User>(_provider, id, default(Int64));
            }

            user.Read(im);

            return user;
        }
    }
}
