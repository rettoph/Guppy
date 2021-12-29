using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Utilities
{
    public class IntervalQueue
    {
        private Queue<(Guid guid, Action<Guid, GameTime> method)> _queue;

        public Interval Interval { get; }

        internal IntervalQueue(Interval interval)
        {
            _queue = new Queue<(Guid guid, Action<Guid, GameTime> method)>();

            this.Interval = interval;

            this.Interval.OnInterval += this.OnInterval;
        }

        public void Enqueue(Guid id, Action<Guid, GameTime> method)
        {
            _queue.Enqueue((id, method));
        }

        private void OnInterval(GameTime gameTime)
        {
            while(_queue.TryDequeue(out (Guid guid, Action<Guid, GameTime> method) action))
            {
                action.method(action.guid, gameTime);
            }
        }
    }
}
