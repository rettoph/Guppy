using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface IGuppyProvider
    {
        event OnEventDelegate<IGuppyProvider, IScoped<IGuppy>>? OnAdded;
        event OnEventDelegate<IGuppyProvider, IScoped<IGuppy>>? OnRemoved;

        IScoped<T> Create<T>()
            where T : class, IGuppy;

        IEnumerable<IScoped<IGuppy>> All();

        IEnumerable<IScoped<T>> All<T>()
            where T : class, IGuppy;
    }
}
