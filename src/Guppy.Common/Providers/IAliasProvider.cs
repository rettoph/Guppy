using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface IAliasProvider
    {
        IEnumerable<Type> GetServiceTypes(Type alias, IServiceProvider provider);
    }
}
