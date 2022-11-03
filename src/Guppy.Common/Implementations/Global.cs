using Guppy.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Implementations
{
    internal sealed class Global<T> : IGlobal<T>
        where T : notnull
    {
        private readonly T _instance;

        public T Instance => _instance;

        public Global(Global global)
        {
            _instance = global.Scope.Instance.GetRequiredService<T>();
        }
    }
}
