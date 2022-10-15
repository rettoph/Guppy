using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IFilter
    {
        bool Invoke(IServiceProvider provider);
    }

    public interface IFilter<in T>
    {
        bool Invoke(IServiceProvider provider, T arg);
    }
}
