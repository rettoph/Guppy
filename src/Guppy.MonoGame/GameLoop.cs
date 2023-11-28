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

namespace Guppy.MonoGame
{
    internal sealed class GameLoop : IGameLoop
    {
        private readonly ImGuiBatch _batch;
        private readonly IGuppyProvider _guppies;
        private readonly List<MonoGameGuppy> _frameables;

        public IGameComponent[] Components { get; private set; }

        public GameLoop(IGuppyProvider guppies, ILifetimeScope lifeTimeScope, IEnumerable<IGameComponent> components)
        {
            _batch = lifeTimeScope.Resolve<ImGuiBatch>();
            _guppies = guppies;
            _frameables = _guppies.OfType<MonoGameGuppy>().ToList();

            _guppies.OnGuppyCreated += HandleGuppyCreated;
            _guppies.OnGuppyDestroyed += HandleGuppyDestroyed;

            this.Components = components.ToArray();
        }

        public void Draw(GameTime gameTime)
        {
            foreach (MonoGameGuppy frameable in _frameables)
            {
                frameable.Draw(gameTime);
            }


            _batch.Begin(gameTime);
            foreach (MonoGameGuppy frameable in _frameables)
            {
                frameable.DrawGui(gameTime);
            }
            _batch.End();
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
