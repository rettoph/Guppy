using Guppy.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    internal sealed class LayerableService : CollectionService<Guid, ILayerable>, ILayerableService
    {
        private ILayerService _layers;

        public LayerableService(ILayerService layers)
        {
            _layers = layers;
        }

        protected override Guid GetKey(ILayerable item)
        {
            return item.Id;
        }

        void ILayerableService.Add(ILayerable layerable)
        {
            this.items.Add(this.GetKey(layerable), layerable);

            this.TryAddToLayer(layerable.LayerKey, layerable);

            layerable.OnLayerKeyChanged += this.HandleItemLayerKeyChanged;
        }

        void ILayerableService.Remove(ILayerable layerable)
        {
            this.items.Remove(this.GetKey(layerable));

            this.TryRemoveFromLayer(layerable.LayerKey, layerable);

            layerable.OnLayerKeyChanged -= this.HandleItemLayerKeyChanged;
        }

        private void TryAddToLayer(string? key, ILayerable layerable)
        {
            if (key is not null && _layers.TryGet(key, out ILayer? layer))
            {
                layer.Add(layerable);
            }
        }

        private void TryRemoveFromLayer(string? key, ILayerable layerable)
        {
            if (key is not null && _layers.TryGet(key, out ILayer? layer))
            {
                layer.Remove(layerable);
            }
        }

        private void HandleItemLayerKeyChanged(ILayerable sender, string? old, string? value)
        {
            this.TryRemoveFromLayer(old, sender);
            this.TryAddToLayer(value, sender);
        }
    }
}
