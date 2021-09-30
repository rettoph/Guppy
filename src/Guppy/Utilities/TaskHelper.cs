using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guppy.Utilities
{
    public static class TaskHelper
    {
        /// <summary>
        /// <para>Create a new Thread that will invoke a given <paramref name="frame"/> method
        /// each <paramref name="millisecondsDelay"/>.</para>
        /// 
        /// <para>If you are not seeing the loops you believe you should, consider calling <see cref="ThreadPool.SetMaxThreads(int, int)"/>.</para>
        /// </summary>
        /// <param name="frame">The action to invoke each interval of the loop.</param>
        /// <param name="millisecondsDelay">The minimum time in milliseconds between each frame</param>
        /// <returns>A cancelation token source for the loop.</returns>
        public static Task CreateLoop(Action<GameTime> frame, Int32 millisecondsDelay, out CancellationTokenSource tokenSource, Object owner = default)
        {
            tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            return Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                DateTime now = DateTime.Now;
                DateTime last = default;
                GameTime gameTime = default;

                while (!token.IsCancellationRequested)
                {
                    Thread.Sleep(millisecondsDelay);

                    last = now;
                    now = DateTime.Now;

                    gameTime = new GameTime(now - start, now - last);

                    frame(gameTime);
                }
            }, token);
        }
    }
}
