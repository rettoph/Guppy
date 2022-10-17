using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    internal sealed class Global<T> : IGlobal<T>, IDisposable
        where T : notnull
    {
        private IScoped<T> _scoped;

        public T Instance => _scoped.Instance;

        public Global(IScoped<T> scoped)
        {
            _scoped = scoped;
        }

        public void Dispose()
        {
            _scoped.Dispose();
        }
    }
}
