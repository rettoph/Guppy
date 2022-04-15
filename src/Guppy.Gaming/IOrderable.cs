using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public interface IOrderable
    {
        int UpdateOrder { get; set; }
        int DrawOrder { get; set; }

        event OnEventDelegate<IFrameable, int> OnUpdateOrderChanged;
        event OnEventDelegate<IFrameable, int> OnDrawOrderChanged;
    }
}
