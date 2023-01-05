using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IFiltered<T>
        where T : class
    {
        public T Instance { get; }
        public IEnumerable<T> Instances { get; }
    }
}
