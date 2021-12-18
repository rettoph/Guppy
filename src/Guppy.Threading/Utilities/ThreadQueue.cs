﻿using Guppy.EntityComponent;
using Guppy.EntityComponent.DependencyInjection;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Guppy.Threading.Utilities
{
    public class ThreadQueue : Service
    {
        #region Private Fields
        private ConcurrentQueue<Action<GameTime>> _queue;
        private Int32 _flushingThreadId;
        private GameTime _gameTime;
        #endregion

        #region Lifecycle Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            _queue = new ConcurrentQueue<Action<GameTime>>();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Enqueue a new <see cref="TThreadAction"/> instance 
        /// to be invoked by the owning thread next frame.
        /// </summary>
        /// <param name="action"></param>
        public void Enqueue(Action<GameTime> action)
        {
            if(Thread.CurrentThread.ManagedThreadId == _flushingThreadId)
            {
                action(_gameTime);
            }
            else if (action is null)
            {
                throw new ArgumentOutOfRangeException(nameof(action));
            }
            else
            {
                _queue.Enqueue(action);
            }
        }

        /// <summary>
        /// Clear the queue and run all contained actions within.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Flush(GameTime gameTime)
        {
            _gameTime = gameTime;
            _flushingThreadId = Thread.CurrentThread.ManagedThreadId;

            while (_queue.TryDequeue(out Action<GameTime> action))
            {
                action(gameTime);
            }

            _flushingThreadId = Int32.MinValue;
        }
        #endregion
    }
}
