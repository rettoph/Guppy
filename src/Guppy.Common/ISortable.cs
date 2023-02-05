using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common
{
    public interface ISortable
    {
        bool GetOrder(Type type, out int order);
    }
}
