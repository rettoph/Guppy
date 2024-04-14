using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Game.MonoGame.Utilities
{
    public sealed class GuppyTimer
    {
        private TimeSpan _interval;
        private readonly GameTime _gameTime;

        public TimeSpan Interval
        {
            get => _interval;
            set => _interval = value;
        }

        public TimeSpan ElapsedTime
        {
            get => _gameTime.ElapsedGameTime;
            set
            {
                TimeSpan diff = _gameTime.ElapsedGameTime - value;

                _gameTime.ElapsedGameTime = value;
                _gameTime.TotalGameTime -= diff;
            }
        }

        public GuppyTimer() : this(TimeSpan.FromSeconds(1))
        {

        }
        public GuppyTimer(TimeSpan interval)
        {
            this.Interval = interval;

            _gameTime = new GameTime();
        }

        public void Update(GameTime gameTime)
        {
            _gameTime.TotalGameTime = gameTime.TotalGameTime;
            _gameTime.ElapsedGameTime += gameTime.ElapsedGameTime;
        }

        public void Reset()
        {
            _gameTime.ElapsedGameTime = TimeSpan.Zero;
        }

        public bool Step([MaybeNullWhen(false)] out GameTime gameTime)
        {
            if (_gameTime.ElapsedGameTime >= _interval)
            {
                _gameTime.ElapsedGameTime -= _interval;

                gameTime = _gameTime;
                return true;
            }

            gameTime = null;
            return false;
        }
    }
}
