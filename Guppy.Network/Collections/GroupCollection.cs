using Guppy.Collections;
using Guppy.Extensions.DependencyInjection;
using Guppy.Network.Groups;
using Guppy.Utilities.Pools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Network.Collections
{
    public class GroupCollection : UniqueCollection<Group>
    {
        private Pool<Group> _groupPool;
        private IServiceProvider _provider;

        public GroupCollection(Pool<Group> groupPool, IServiceProvider provider) : base(provider)
        {
            _groupPool = groupPool;
        }

        public Group GetOrCreateById(Guid id, Action<Group> setup = null)
        {
            var stored = this.GetById<Group>(id);
            if (stored == default(Group))
            { // Pull a new group from the pool...
                stored = _groupPool.Pull(_provider, g =>
                {
                    g.SetId(id);
                    setup?.Invoke(g);
                });
            }

            return stored;
        }
    }
}
