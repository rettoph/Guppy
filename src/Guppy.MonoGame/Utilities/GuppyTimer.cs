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

        public bool Step(GameTime gameTime, bool locked, [MaybeNullWhen(false)] out TimeSpan elapsedTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime >= _interval && !locked)
            {
                elapsedTime = _elapsedTime;
                _elapsedTime -= _interval;

                return true;
            }

            elapsedTime = TimeSpan.Zero;
            return false;
        }
    }
}
