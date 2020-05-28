using Guppy.Collections;
using Guppy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Collections
{
    public class NetworkServiceCollection<TService> : ProtectedServiceCollection<TService>
        where TService : IService
    {
        internal Boolean TryAdd(TService item)
            => this.Add(item);

        internal Boolean TryRemove(TService item)
            => this.Remove(item);
    }
}
