using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Guppy.Threading.Helpers
{
    public static class ThreadHelper
    {
        public static void CreateLoop(Action<GameTime> frame, Int32 millisecondsDelay, CancellationToken token)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
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
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"{e.Message}\n{e.StackTraceEx()}");
                }
            }));

            thread.Start();
        }
    }
}
