using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;

namespace Guppy.Game.MonoGame.Common.Utilities
{
    public sealed class GuppyTimer(TimeSpan interval)
    {
        private readonly GameTime _gameTime = new();

        public TimeSpan Interval { get; set; } = interval;

        public TimeSpan ElapsedTime
        {
            get => this._gameTime.ElapsedGameTime;
            set
            {
                TimeSpan diff = this._gameTime.ElapsedGameTime - value;

                this._gameTime.ElapsedGameTime = value;
                this._gameTime.TotalGameTime -= diff;
            }
        }

        public GuppyTimer() : this(TimeSpan.FromSeconds(1))
        {

        }

        public void Update(GameTime gameTime)
        {
            this._gameTime.TotalGameTime = gameTime.TotalGameTime;
            this._gameTime.ElapsedGameTime += gameTime.ElapsedGameTime;
        }

        public void Reset() => this._gameTime.ElapsedGameTime = TimeSpan.Zero;

        public bool Step([MaybeNullWhen(false)] out GameTime gameTime)
        {
            if (this._gameTime.ElapsedGameTime >= this.Interval)
            {
                this._gameTime.ElapsedGameTime -= this.Interval;

                gameTime = this._gameTime;
                return true;
            }

            gameTime = null;
            return false;
        }
    }
}