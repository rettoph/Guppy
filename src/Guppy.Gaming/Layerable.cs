using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming
{
    public abstract class Layerable : Frameable, ILayerable
    {
        private string? _layerKey;

        public string? LayerKey
        {
            get => _layerKey;
            set => this.OnLayerKeyChanged!.InvokeIf(value != _layerKey, this, ref _layerKey, value);
        }

        public event OnChangedEventDelegate<ILayerable, string?>? OnLayerKeyChanged;
    }
}
