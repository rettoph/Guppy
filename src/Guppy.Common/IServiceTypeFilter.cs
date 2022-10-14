using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IServiceTypeFilter<T> : IFilter<IServiceProvider>
        where T : class
    {
        int Order { get; }
        Type ImplementationType { get; }
    }
}
