using Microsoft.Xna.Framework;

namespace Guppy.Game.Helpers
{
    public static class TaskHelper
    {
        /// <summary>
        /// <para>Create a new Thread that will invoke a given <paramref name="frame"/> method
        /// each <paramref name="interval"/>.</para>
        /// 
        /// <para>If you are not seeing the loops you believe you should, consider calling <see cref="ThreadPool.SetMaxThreads(int, int)"/>.</para>
        /// </summary>
        /// <param name="frame">The action to invoke each interval of the loop.</param>
        /// <param name="interval">The minimum time in milliseconds between each frame</param>
        /// <param name="token">a cancellation token</param>
        /// <returns>A cancelation token source for the loop.</returns>
        public static async Task CreateLoop(Action<GameTime> frame, TimeSpan interval, CancellationToken token)
        {
            DateTime start = DateTime.Now;
            DateTime now = start;
            GameTime gameTime = new();
            PeriodicTimer _timer = new(interval);

            while (await _timer.WaitForNextTickAsync(token) && !token.IsCancellationRequested)
            {
                DateTime last = now;
                now = DateTime.Now;

                gameTime.TotalGameTime = now - start;
                gameTime.ElapsedGameTime = now - last;

                frame(gameTime);
            }
        }
    }
}