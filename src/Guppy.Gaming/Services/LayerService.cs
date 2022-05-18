using Guppy.Services.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    internal sealed class LayerService : CollectionService<string, ILayer>, ILayerService
    {
        protected override string GetKey(ILayer item)
        {
            return item.Key;
        }

        public void Update(GameTime gameTime)
        {
            foreach(ILayer layer in this.items.Values)
            {
                if(layer.Enabled)
                {
                    layer.Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (ILayer layer in this.items.Values)
            {
                if(layer.Visible)
                {
                    layer.Draw(gameTime);
                }
            }
        }

        void ILayerService.Add(ILayer layer)
        {
            this.items.Add(this.GetKey(layer), layer);
        }

        void ILayerService.Remove(ILayer layer)
        {
            this.items.Remove(this.GetKey(layer));
        }
    }
}
