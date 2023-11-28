using Guppy.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Guppy.MonoGame.Common;
using Guppy.Common;
using Guppy.GUI;
using Autofac;
using Guppy.Common.Extensions;
using Guppy.MonoGame.Common.Enums;
using Guppy.Enums;

namespace Guppy.MonoGame
{
    internal sealed class GameLoop : IGameLoop
    {
        private IDrawableComponent[] _drawableComponents;
        private IUpdateableComponent[] _updateableComonents;

        public GameLoop(IEnumerable<IGlobalComponent> components)
        {
            _drawableComponents = components.OfType<IDrawableComponent>().Sequence(DrawSequence.Draw).ToArray();
            _updateableComonents = components.OfType<IUpdateableComponent>().Sequence(UpdateSequence.Update).ToArray();
        }

        public void Draw(GameTime gameTime)
        {
            foreach(IDrawableComponent component in _drawableComponents)
            {
                component.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (IUpdateableComponent component in _updateableComonents)
            {
                component.Update(gameTime);
            }
        }
    }
}
