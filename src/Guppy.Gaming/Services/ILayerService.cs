using Guppy.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Gaming.Services
{
    public interface ILayerService : ICollectionService<string, ILayer>
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);

        internal void Add(ILayer layer);
        internal void Remove(ILayer layer);
    }
}
