using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    public class Interval
    {
        #region Private Fields
        private readonly ActionTimer _actionTimer;
        #endregion

        #region Constructor
        public Interval(Double milliseconds)
        {
            _actionTimer = new ActionTimer(milliseconds);
        }
        #endregion

        #region Events
        public delegate void IntervalDelegate(GameTime gameTime);

        public event IntervalDelegate OnInterval;
        #endregion

        #region Methods
        internal void Update(GameTime gameTime)
        {
            _actionTimer.Update(gameTime, this.InvokeDelegate);
        }

        private void InvokeDelegate(GameTime gameTime)
        {
            this.OnInterval?.Invoke(gameTime);
        }
        #endregion
    }
}
