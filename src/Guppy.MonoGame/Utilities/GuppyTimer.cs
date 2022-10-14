using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Utilities
{
    public sealed class GuppyTimer
    {
        private TimeSpan _interval;
        private GameTime _gameTime;

        public TimeSpan Interval
        {
            get => _interval;
            set => _interval = value;
        }

        public TimeSpan ElapsedTime => _gameTime.ElapsedGameTime;

        public GuppyTimer(TimeSpan interval)
        {
            this.Interval = interval;

            _gameTime = new GameTime();
        }

        public void Step(GameTime gameTime, Func<GameTime, bool> step)
        {
            _gameTime.TotalGameTime = gameTime.TotalGameTime;
            _gameTime.ElapsedGameTime += gameTime.ElapsedGameTime;

            while (_gameTime.ElapsedGameTime >= _interval && step(_gameTime))
            {
                _gameTime.ElapsedGameTime -= _interval;
            }
        }
    }
}
