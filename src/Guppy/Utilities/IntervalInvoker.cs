using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Utilities
{
    /// <summary>
    /// A simple container of action timers with which a delegate can be subscrbed to.
    /// </summary>
    public class IntervalInvoker
    {
        #region Private Fields
        private Dictionary<Double, Interval> _intervals;
        #endregion

        #region Public Properties
        public Interval this[Double milliseconds]
        {
            get
            {
                if (!_intervals.TryGetValue(milliseconds, out Interval interval))
                {
                    interval = new Interval(milliseconds);
                    _intervals.Add(milliseconds, interval);
                }

                return interval;
            }
        }
        #endregion

        #region Constructor
        public IntervalInvoker()
        {
            _intervals = new Dictionary<Double, Interval>();
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            foreach(Interval interval in _intervals.Values)
                interval.Update(gameTime);
        }
        #endregion
    }
}
