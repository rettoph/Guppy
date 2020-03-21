using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Attributes;
using Guppy.Enums;

namespace Guppy
{
    /// <summary>
    /// Basic "Queue" to contain unplanned/unexpected updates.
    /// 
    /// This queue should be flushed and all contained updates
    /// excecuted in the owning Game PostUpdate method.
    /// 
    /// Example uses: Multi threaded actions (network updates),
    /// unexpected cleans, ect
    /// </summary>
    [Service(Lifetime.TypedScoped)]
    public sealed class UpdateBuffer : Service
    {
        #region Private Fields
        private ConcurrentQueue<Action<GameTime>> _queue;
        private Action<GameTime> _update;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _queue = new ConcurrentQueue<Action<GameTime>>();
        }

        protected override void Dispose()
        {
            base.Dispose();

            while (_queue.Any()) // Empty the queue out
                _queue.TryDequeue(out _update);
        }
        #endregion

        #region Helper Methods
        public void Enqueue(Action<GameTime> update)
        {
            _queue.Enqueue(update);
        }

        /// <summary>
        /// Excecute all enqueued updates.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Flush(GameTime gameTime)
        {
            while (_queue.Any())
                if (_queue.TryDequeue(out _update))
                    _update(gameTime);
        }
        #endregion
    }
}
