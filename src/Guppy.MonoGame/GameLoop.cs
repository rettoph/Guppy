using Guppy.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame
{
    public sealed class GameLoop : MonoGameGuppy
    {
        private IGuppyProvider _guppies;
        private List<MonoGameGuppy> _frameables;

        public GameLoop(IGuppyProvider guppies)
        {
            _guppies = guppies;
            _frameables = _guppies.OfType<MonoGameGuppy>().Where(x => x is not GameLoop).ToList();

            _guppies.OnGuppyCreated += HandleGuppyCreated;
            _guppies.OnGuppyDestroyed += HandleGuppyDestroyed;
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
            if (args is MonoGameGuppy frameable && args is not GameLoop)
            {
                _frameables.Add(frameable);
            }
        }

        private void HandleGuppyDestroyed(IGuppyProvider sender, IGuppy args)
        {
            if (args is MonoGameGuppy frameable && args is not GameLoop)
            {
                _frameables.Remove(frameable);
            }
        }
    }
}
