using Guppy.Attributes;
using Guppy.Common.Attributes;
using Guppy.GUI;
using Guppy.MonoGame.Common;
using Guppy.MonoGame.Common.Enums;
using Guppy.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    [Sequence<DrawSequence>(DrawSequence.PostDraw)]
    internal class DrawGuiComponent : GlobalComponent, IDrawableComponent
    {
        private readonly ImGuiBatch _batch;
        private readonly IGuppyProvider _guppies;
        private readonly List<MonoGameGuppy> _frameables;

        public DrawGuiComponent(IGuppyProvider guppies, ImGuiBatch batch)
        {
            _batch = batch;
            _guppies = guppies;
            _frameables = _guppies.OfType<MonoGameGuppy>().ToList();

            _guppies.OnGuppyCreated += HandleGuppyCreated;
            _guppies.OnGuppyDestroyed += HandleGuppyDestroyed;
        }

        public void Initialize()
        {
        }

        public void Draw(GameTime gameTime)
        {
            _batch.Begin(gameTime);
            foreach (MonoGameGuppy frameable in _frameables)
            {
                frameable.DrawGui(gameTime);
            }
            _batch.End();
        }

        private void HandleGuppyCreated(IGuppyProvider sender, IGuppy args)
        {
            if (args is MonoGameGuppy frameable)
            {
                _frameables.Add(frameable);
            }
        }

        private void HandleGuppyDestroyed(IGuppyProvider sender, IGuppy args)
        {
            if (args is MonoGameGuppy frameable)
            {
                _frameables.Remove(frameable);
            }
        }
    }
}
