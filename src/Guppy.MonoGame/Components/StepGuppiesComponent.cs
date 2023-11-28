using Guppy.Attributes;
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
    internal class StepGuppiesComponent : GameLoopComponent, IDrawableComponent, IUpdateableComponent
    {
        private readonly IGuppyProvider _guppies;
        private readonly List<MonoGameGuppy> _frameables;

        public StepGuppiesComponent(IGuppyProvider guppies)
        {
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
            foreach (MonoGameGuppy frameable in _frameables)
            {
                frameable.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (MonoGameGuppy frameable in _frameables)
            {
                frameable.Update(gameTime);
            }
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
