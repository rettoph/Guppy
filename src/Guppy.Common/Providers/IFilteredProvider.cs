using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Providers
{
    public interface IFilteredProvider
    {
        IFiltered<T> Get<T>()
            where T : class;

        T? Instance<T>()
            where T : class;

        IEnumerable<T> Instances<T>()
            where T : class;

        IFiltered<T> Get<T>(params IState[] states)
            where T : class;

        T? Instance<T>(params IState[] states)
            where T : class;

        IEnumerable<T> Instances<T>(params IState[] states)
            where T : class;
    }
}
