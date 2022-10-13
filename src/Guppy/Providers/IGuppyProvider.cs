using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface IGuppyProvider : IEnumerable<IScoped<IGuppy>>
    {
        IScoped<T> Create<T>()
            where T : class, IGuppy;

        IEnumerable<IScoped<T>> All<T>()
            where T : class, IGuppy;
    }
}
