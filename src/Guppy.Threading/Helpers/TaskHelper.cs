using Guppy.Threading.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guppy.Threading.Helpers
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
        /// <param name="token">a cancellation token</param>
        /// <returns>A cancelation token source for the loop.</returns>
        public static Task CreateLoop(Action<GameTime> frame, Int32 millisecondsDelay, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                try
                {
                    DateTime start = DateTime.Now;
                    DateTime now = DateTime.Now;
                    DateTime last = default;
                    GameTime gameTime = new GameTime();

                    while (!token.IsCancellationRequested)
                    {
                        await Task.Delay(millisecondsDelay);

                        last = now;
                        now = DateTime.Now;

                        gameTime.TotalGameTime = now - start;
                        gameTime.ElapsedGameTime = now - last;

                        frame(gameTime);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"{e.Message}\n{e.StackTraceEx()}");
                }
            }, token).Log();
        }
    }
}
