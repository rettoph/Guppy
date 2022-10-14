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
        private TimeSpan _elapsedTime;
        private TimeSpan _interval;

        public TimeSpan Interval
        {
            get => _interval;
            set => _interval = value;
        }

        public TimeSpan ElapsedTime => _elapsedTime;

        public GuppyTimer(TimeSpan interval)
        {
            this.Interval = interval;
        }

        public bool Step(GameTime gameTime, out int count, [MaybeNullWhen(false)] out TimeSpan elapsedTime)
        {
            return this.Step(gameTime, false, out count, out elapsedTime);
        }

        public bool Step(GameTime gameTime, bool locked, out int count, [MaybeNullWhen(false)] out TimeSpan elapsedTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (locked || _elapsedTime < _interval)
            {
                elapsedTime = TimeSpan.Zero;
                count = 0;
                return false;
            }

            count = (int)Math.Floor(_elapsedTime / _interval);
            elapsedTime = _elapsedTime;
            _elapsedTime -= _interval * count;

            return true;
        }
    }
}
