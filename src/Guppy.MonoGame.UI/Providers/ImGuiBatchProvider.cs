using Guppy.Common;
using Guppy.MonoGame.UI.Loaders;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI.Providers
{
    internal sealed class ImGuiBatchProvider : IImGuiBatchProvider
    {
        private readonly GameWindow _window;
        private readonly GraphicsDevice _graphics;
        private readonly IBus _bus;
        private readonly IResourceProvider _resources;
        private readonly Dictionary<string, ImGuiBatch> _batches;

        public ImGuiBatchProvider(
            GameWindow window, 
            GraphicsDevice graphics, 
            IBus bus, 
            IResourceProvider resources,
            IEnumerable<IImGuiBatchLoader> loaders)
        {
            _window = window;
            _graphics = graphics;
            _bus = bus;
            _resources = resources;
            _batches = new Dictionary<string, ImGuiBatch>();

            foreach (IImGuiBatchLoader loader in loaders)
            {
                loader.Load(this);
            }

            foreach(var batch in _batches.Values)
            {
                batch.RebuildFontAtlas();
            }
        }

        public ImGuiBatch Get(string name)
        {
            if(!_batches.TryGetValue(name, out var batch))
            {
                batch = this.Create();
                _batches[name] = batch;
            }

            return batch;
        }

        private ImGuiBatch Create()
        {
            return new ImGuiBatch(_window, _graphics, _bus, _resources);
        }
    }
}
