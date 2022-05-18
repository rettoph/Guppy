using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public interface ILayer : IFrameable, IEnumerable<ILayerable>
    {
        string Key { get; }
        int UpdateOrder { get; set; }
        int DrawOrder { get; set; }

        event OnChangedEventDelegate<ILayer, int>? OnUpdateOrderChanged;
        event OnChangedEventDelegate<ILayer, int>? OnDrawOrderChanged;

        internal void Add(ILayerable item);
        internal void Remove(ILayerable item);
    }
}
