using Guppy.Factories;
using Guppy.Network.Groups;
using Guppy.Pooling.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Factories
{
    public class GroupFactory : InitializableFactory<Group>
    {
        public GroupFactory(IPoolManager<Group> pools, IServiceProvider provider) : base(pools, provider)
        {
        }
    }
}
