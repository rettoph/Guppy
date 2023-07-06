using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface IOptions<T>
        where T : new()
    {
        public T Value { get; }
    }
}
