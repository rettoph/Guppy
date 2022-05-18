using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public interface ILayerable : IFrameable
    {
        string? LayerKey { get; set; }

        event OnChangedEventDelegate<ILayerable, string?>? OnLayerKeyChanged;
    }
}
