using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public interface IScoped<T>
    {
        public T Instance { get; }
    }
}
