using Guppy.Factories;
using Guppy.Network.Security;
using Guppy.Pooling.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Factories
{
    public class UserFactory : InitializableFactory<User>
    {
        public UserFactory(IPoolManager<User> pools, IServiceProvider provider) : base(pools, provider)
        {

        }
    }
}
