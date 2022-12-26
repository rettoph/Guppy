using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface IFilteredProvider
    {
        IFiltered<T> Get<T>(object? configuration)
            where T : class;

        T? Instance<T>(object? configuration = null)
            where T : class;

        IEnumerable<T> Instances<T>(object? configuration = null)
            where T : class;
    }
}
