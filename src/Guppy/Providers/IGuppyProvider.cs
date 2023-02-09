using Guppy.Common;
using Guppy.Common.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface IGuppyProvider : ICollectionManager<IGuppy>
    {
        T Create<T>()
            where T : class, IGuppy;
    }
}
