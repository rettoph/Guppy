﻿using Microsoft.Xna.Framework;
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

        public void Step(Func<GameTime, bool> step)
        {
            while (_gameTime.ElapsedGameTime >= _interval && step(_gameTime))
            {
                _gameTime.ElapsedGameTime -= _interval;

                step(_gameTime);
            }
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