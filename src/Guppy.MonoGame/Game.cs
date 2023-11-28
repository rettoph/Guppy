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
    internal sealed class Game : IGame
    {
        private IDrawableComponent[] _drawableComponents;
        private IUpdateableComponent[] _updateableComonents;

        public IGuppyProvider Guppies { get; private set; }

        public Game(IGuppyProvider guppies, IEnumerable<IGlobalComponent> components)
        {
            this.Guppies = guppies;

            _drawableComponents = components.OfType<IDrawableComponent>().Sequence(DrawSequence.Draw).ToArray();
            _updateableComonents = components.OfType<IUpdateableComponent>().Sequence(UpdateSequence.Update).ToArray();
        }

        public void Initialize()
        {
            this.Guppies.Initialize();
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
