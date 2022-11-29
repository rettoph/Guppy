using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Collections
{
    public interface IManagedCollection
    {
        Type Type { get; }

        void Initialize(CollectionManager manager);

        bool TryAdd(object item);

        bool TryRemove(object item);
    }
}
