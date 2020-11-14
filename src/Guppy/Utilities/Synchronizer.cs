using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guppy.Attributes;
using Guppy.Enums;
using Guppy.DependencyInjection;

namespace Guppy.Utilities
{
    /// <summary>
    /// Basic "Queue" to contain unplanned/unexpected updates.
    /// 
    /// This queue should be flushed and all contained updates
    /// excecuted in the owning Scene PostUpdate method.
    /// 
    /// Example uses: Multi threaded actions (network updates),
    /// unexpected cleans, ect
    /// </summary>
    public sealed class Synchronizer : Service
    {
        #region Private Fields
        private Queue<Action<GameTime>> _queue;
        #endregion

        #region Lifecycle Methods
        protected override void Initialize(ServiceProvider provider)
        {
            base.Initialize(provider);

            _queue = new Queue<Action<GameTime>>();
        }

        protected override void Release()
        {
            base.Release();

            while (_queue.Any()) // Empty the queue out
                _queue.Dequeue();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Enqueue an action to be ran 
        /// </summary>
        /// <param name="update"></param>
        /// <param name="next"></param>
        public void Enqueue(Action<GameTime> update)
            => _queue.Enqueue(update);

        /// <summary>
        /// Excecute all enqueued updates.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Flush(GameTime gameTime)
        {
            while (_queue.Any())
                _queue.Dequeue()(gameTime);
        }
        #endregion
    }
}
