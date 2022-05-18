using Guppy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    public interface ILayerableService : ICollectionService<Guid, ILayerable>
    {
        internal void Add(ILayerable layerable);
        internal void Remove(ILayerable layerable);
    }
}
